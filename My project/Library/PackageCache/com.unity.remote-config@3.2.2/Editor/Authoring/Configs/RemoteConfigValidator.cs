using System.Collections.Generic;
using System.Linq;
using Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Configs
{
    class RemoteConfigValidator : IRemoteConfigValidator
    {
        public bool Validate(
            Dictionary<RemoteConfigEntry, RemoteConfigFile> entryToFileMap,
            ICollection<RemoteConfigDeploymentException> deploymentExceptions)
        {
            if (!IsJsonValidBetweenFiles(entryToFileMap, deploymentExceptions))
            {
                return false;
            }
            
            return true;
        }

        static bool IsJsonValidBetweenFiles(
            Dictionary<RemoteConfigEntry, RemoteConfigFile> entryToFileMap,
            ICollection<RemoteConfigDeploymentException> deploymentExceptions)
        {
            var isValid = true;
            
            foreach (var entry in entryToFileMap.Keys)
            {
                var containingFiles = entryToFileMap
                    .Values
                    .Where(file => file.Content.Entries.ContainsKey(entry.Key))
                    .ToHashSet();

                if (containingFiles.Count > 1)
                {
                    deploymentExceptions.Add(
                        new DuplicateKeysInMultipleFilesException(
                            entry.Key,
                            containingFiles));
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
