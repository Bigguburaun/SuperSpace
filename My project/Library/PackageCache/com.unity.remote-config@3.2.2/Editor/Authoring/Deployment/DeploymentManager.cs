using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Unity.Services.DeploymentApi.Editor;
using Unity.Services.RemoteConfig.Authoring.Editor.Analytics;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling;
using Unity.Services.RemoteConfig.Authoring.Editor.Formatting;
using Unity.Services.RemoteConfig.Authoring.Editor.Networking;
using Unity.Services.RemoteConfig.Authoring.Shared.Collections;
using Logger = Unity.Services.RemoteConfig.Authoring.Shared.Logging.Logger;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Deployment
{
    class DeploymentManager : IDeploymentManager
    {
        readonly IWebApiClient m_WebApiClient;
        readonly IDeploymentInfo m_DeploymentInfo;
        readonly IRemoteConfigParser m_RemoteConfigParser;
        readonly IRemoteConfigValidator m_RemoteConfigValidator;
        readonly IFormatValidator m_FormatValidator;
        readonly IConfigAnalytics m_ConfigAnalytics;

        public DeploymentManager(IDeploymentInfo deploymentInfo,
            IWebApiClient webApiClient,
            IRemoteConfigParser remoteConfigParser,
            IRemoteConfigValidator remoteConfigValidator,
            IFormatValidator formatValidator,
            IConfigAnalytics configAnalytics)
        {
            m_WebApiClient = webApiClient;
            m_DeploymentInfo = deploymentInfo;
            m_RemoteConfigParser = remoteConfigParser;
            m_RemoteConfigValidator = remoteConfigValidator;
            m_FormatValidator = formatValidator;
            m_ConfigAnalytics = configAnalytics;
        }

        public async Task DeployAsync(IReadOnlyList<RemoteConfigFile> configFiles)
        {
            Exception exceptionToThrow = null;
            
            SetDeployingStatus(configFiles);
            
            configFiles.ForEach(configFile => configFile.TrySetContentFromPath(configFile.Path));

            var deploymentExceptions = new List<RemoteConfigDeploymentException>();
            var formatValidConfigs = configFiles
                .Where(configFile => m_FormatValidator.Validate(configFile, deploymentExceptions))
                .ToList();

            var entryToFileMap = new Dictionary<RemoteConfigEntry, RemoteConfigFile>();
            if (formatValidConfigs.Any())
            {
                entryToFileMap = m_RemoteConfigParser.ParseFiles(formatValidConfigs);
                ValidatePayload(entryToFileMap, deploymentExceptions);
            }
            
            var validEntries = entryToFileMap.Keys.Where(
                entry => !deploymentExceptions.Exists(deploymentException => 
                    deploymentException.AffectedFiles.Contains(entryToFileMap[entry])));

            if (validEntries.Any())
            {
                var filesToDeploy = GetUnaffectedFiles(configFiles, deploymentExceptions);

                try
                {
                    await DeployInternalAsync(validEntries);
                    SetDeployedStatus(filesToDeploy);
                }
                catch (RemoteConfigDeploymentException e)
                {
                    deploymentExceptions.Add(e);
                    e.AffectedFiles.AddRange(filesToDeploy);
                }
                catch (Exception e)
                {
                    filesToDeploy.ForEach(configFile =>
                    {
                        configFile.Status = new DeploymentStatus("Deploy Failed", e.Message, SeverityLevel.Error);
                        configFile.Progress = 0f;
                    });
                    
                    exceptionToThrow = e;
                }
            }

            var validConfigs = entryToFileMap.Values.Where(
                configFile => !deploymentExceptions.Exists(
                    deploymentException => deploymentException.AffectedFiles.Contains(configFile)));
            m_ConfigAnalytics.SendDeployedEvent(configFiles.Count, validConfigs);
            
            if (deploymentExceptions.Any())
            {
                SetDeploymentErrorStatus(deploymentExceptions);
                exceptionToThrow = exceptionToThrow ?? deploymentExceptions.First();
            }

            if (exceptionToThrow != null)
            {
                throw exceptionToThrow;
            }
        }

        async Task DeployInternalAsync(IEnumerable<RemoteConfigEntry> configEntries)
        {
            Logger.LogVerbose("Fetching remote config");
            var fetchResult = await GetRemoteConfigsAsync();
            Logger.LogVerbose($"Fetching result: {fetchResult}");
            if (fetchResult.ConfigsExist)
            {
                var configsToDeploy = MergeConfigs(configEntries, fetchResult.ConfigsValue);
                await UpdateConfigAsync(configsToDeploy, fetchResult.ConfigId);
            }
            else
            {
                await CreateConfigAsync(configEntries);
            }
        }

        void ValidatePayload(
            Dictionary<RemoteConfigEntry, RemoteConfigFile> entryToFileMap,
            ICollection<RemoteConfigDeploymentException> deploymentExceptions)
        {
            m_RemoteConfigValidator.Validate(entryToFileMap, deploymentExceptions);
        }
        
        async Task<FetchResult> GetRemoteConfigsAsync()
        {
            return await m_WebApiClient.Fetch(m_DeploymentInfo.CloudProjectId, m_DeploymentInfo.EnvironmentId);
        }

        async Task CreateConfigAsync(IEnumerable<RemoteConfigEntry> remoteConfigEntries)
        {
            Logger.LogVerbose($"Create config async. Deployment info: {m_DeploymentInfo}");
            var configId = await m_WebApiClient.Post(
                m_DeploymentInfo.CloudProjectId, 
                m_DeploymentInfo.EnvironmentId, 
                JArray.FromObject(remoteConfigEntries));
        }

        async Task UpdateConfigAsync(IEnumerable<RemoteConfigEntry> remoteConfigEntries, string configId)
        {
            Logger.LogVerbose($"Update config async. Deployment info: {m_DeploymentInfo}. Config id: {configId}.");
            await m_WebApiClient.Put(
                m_DeploymentInfo.CloudProjectId, 
                m_DeploymentInfo.EnvironmentId, 
                configId, 
                JArray.FromObject(remoteConfigEntries));
        }

        List<RemoteConfigFile> GetUnaffectedFiles(
            IReadOnlyList<RemoteConfigFile> files,
            List<RemoteConfigDeploymentException> deploymentExceptions)
        {
            var unaffectedFiles = new List<RemoteConfigFile>();

            foreach (var file in files)
            {
                if (!deploymentExceptions.Any(ex => ex.AffectedFiles.Contains(file)))
                {
                    unaffectedFiles.Add(file);
                }
            }
            
            return unaffectedFiles;
        }

        static IEnumerable<RemoteConfigEntry> MergeConfigs(IEnumerable<RemoteConfigEntry> clientConfigs, JArray configsFromRemote)
        {
            var clientConfigsList = clientConfigs.ToList();
            var remoteConfigsList = 
                configsFromRemote?.ToObject<List<RemoteConfigEntry>>() 
                ?? new List<RemoteConfigEntry>();

            var clientKeys = clientConfigsList.Select(config => config.Key).ToList();
            var remoteKeys = remoteConfigsList.Select(config => config.Key).ToList();

            var conflicts = clientKeys.Intersect(remoteKeys);
            var cleanedUpConfigsFromRemote = 
                remoteConfigsList.Where(token => !conflicts.Contains(token.Key));

            var finalConfigs = new List<RemoteConfigEntry>();
            finalConfigs.AddRange(clientConfigsList);
            finalConfigs.AddRange(cleanedUpConfigsFromRemote);
            return finalConfigs;
        }
        
        static void SetDeployingStatus(IReadOnlyCollection<RemoteConfigFile> files)
        {
            files.ForEach(config =>
            {
                config.Status =
                    new DeploymentStatus(
                        "Deploying",
                        string.Empty,
                        SeverityLevel.Info);
                config.Progress = 0f;
            });
        }

        static void SetDeployedStatus(IReadOnlyCollection<RemoteConfigFile> files)
        {
            files.ForEach(configFile =>
            {
                if (configFile.Status.MessageSeverity == SeverityLevel.Error)
                {
                    return;
                }
                
                configFile.Status = new DeploymentStatus(
                    "Deployed",
                    "Deployed Successfully",
                    SeverityLevel.Info);
                configFile.Progress = 100f;
            });
        }

        static void SetDeploymentErrorStatus(
            IReadOnlyCollection<RemoteConfigDeploymentException> deploymentExceptions)
        {
            deploymentExceptions.ForEach(deploymentException =>
            {
                deploymentException.AffectedFiles.ForEach(configFile =>
                {
                    configFile.Status = new DeploymentStatus(
                        deploymentException.StatusDescription,
                        deploymentException.StatusDetail);
                });
            });
        }
    }
}
