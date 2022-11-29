#if NUGET_MOQ_AVAILABLE
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Unity.Services.RemoteConfig.Authoring.Editor.Analytics;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using Unity.Services.RemoteConfig.Authoring.Editor.Deployment;
using Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling;
using Unity.Services.RemoteConfig.Authoring.Editor.Formatting;
using Unity.Services.RemoteConfig.Authoring.Editor.Networking;
using Unity.Services.RemoteConfig.Tests.Editor.Authoring.Configs;
using Unity.Services.RemoteConfig.Tests.Editor.Authoring.Shared;
using UnityEngine.TestTools;

namespace Unity.Services.RemoteConfig.Tests.Editor.Authoring.Deployment
{
    public class DeploymentManagerTests
    {
        const string k_MockEnvironmentId = "environmentId";
        const string k_MockCloudProjectId = "cloudProjectId";

        Dictionary<RemoteConfigEntry, RemoteConfigFile> m_EntryToFileMap;
        Mock<IWebApiClient> m_MockApi;
        Mock<IDeploymentInfo> m_MockDeploymentInfo;
        Mock<IRemoteConfigParser> m_MockParser;
        Mock<IRemoteConfigValidator> m_MockValidator;
        Mock<IFormatValidator> m_MockFormatValidator;
        Mock<IConfigAnalytics> m_MockAnalytics;
        DeploymentManager m_DeploymentManager;
        List<RemoteConfigFile> m_ConfigFiles;

        [SetUp]
        public void SetUp()
        {
            m_EntryToFileMap = new Dictionary<RemoteConfigEntry, RemoteConfigFile>();
            m_MockApi = new Mock<IWebApiClient>();
            m_MockDeploymentInfo = new Mock<IDeploymentInfo>();
            m_MockParser = new Mock<IRemoteConfigParser>();
            m_MockValidator = new Mock<IRemoteConfigValidator>();
            m_MockFormatValidator = new Mock<IFormatValidator>();
            m_ConfigFiles = new List<RemoteConfigFile>();
            m_ConfigFiles.Add(TestFileGetter.GetTestFile(AssetFilePaths.ValidBase));
            m_MockApi
                .Setup(api => 
                    api.Post(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<JArray>()))
                .Returns(Task.FromResult("return"));
            m_MockApi
                .Setup(api => 
                    api.Put(
                        It.IsAny<string>(), 
                        It.IsAny<string>(), 
                        It.IsAny<string>(), 
                        It.IsAny<JArray>()))
                .Returns(Task.CompletedTask);
            m_MockFormatValidator
                .Setup(
                    validator => validator.Validate(
                        It.IsAny<RemoteConfigFile>(), 
                        It.IsAny<ICollection<RemoteConfigDeploymentException>>()))
                .Returns(true);
            m_MockValidator
                .Setup(
                    validator => validator.Validate(
                        It.IsAny<Dictionary<RemoteConfigEntry, RemoteConfigFile>>()
                        , It.IsAny<ICollection<RemoteConfigDeploymentException>>()))
                .Returns(true);
            m_MockDeploymentInfo
                .SetupGet(info => info.EnvironmentId)
                .Returns(k_MockEnvironmentId);
            m_MockDeploymentInfo
                .SetupGet(info => info.CloudProjectId)
                .Returns(k_MockCloudProjectId);
            m_MockParser
                .Setup(parser => parser.ParseFiles(It.IsAny<IEnumerable<RemoteConfigFile>>()))
                .Returns(m_EntryToFileMap);
            m_MockAnalytics = new Mock<IConfigAnalytics>();
            m_MockAnalytics
                .Setup(a => 
                    a.SendDeployedEvent(
                        It.IsAny<int>(), 
                        It.IsAny<IEnumerable<RemoteConfigFile>>()))
                .Verifiable();
            var remoteConfigEntry = new RemoteConfigEntry()
            {
                Key = "key",
                Type = "STRING",
                Value = JToken.FromObject("value")
            };
            m_EntryToFileMap.Add(remoteConfigEntry, new RemoteConfigFile());
            
            m_DeploymentManager = new DeploymentManager(m_MockDeploymentInfo.Object,
                m_MockApi.Object,
                m_MockParser.Object,
                m_MockValidator.Object,
                m_MockFormatValidator.Object,
                m_MockAnalytics.Object);
        }


        [UnityTest]
        public IEnumerator ConfigDoesNotExist_Posts() => AsyncTest.AsCoroutine(ConfigDoesNotExist_PostsAsync);
        async Task ConfigDoesNotExist_PostsAsync()
        {
            var config = new JObject();
            m_MockApi
                .Setup(api => api.Fetch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new FetchResult(config)));

            await m_DeploymentManager.DeployAsync(m_ConfigFiles);
            m_MockApi.Verify(api => 
                    api.Post(
                        It.IsAny<string>(),
                        It.IsAny<string>(), 
                        It.IsAny<JArray>()), 
                Times.Once());
            m_MockApi.Verify(api => api.Put(k_MockCloudProjectId,
                    k_MockEnvironmentId,
                    It.IsAny<string>(),
                    It.IsAny<JArray>()),
                Times.Never());
        }

        [UnityTest]
        public IEnumerator ConfigExists_Puts() => AsyncTest.AsCoroutine(ConfigExists_PutsAsync);
        async Task ConfigExists_PutsAsync()
        {
            var config = new JObject();
            config["type"] = "settings";
            config["id"] = "id";

            m_MockApi
                .Setup(api => api.Fetch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new FetchResult(config)));

            await m_DeploymentManager.DeployAsync(m_ConfigFiles);
            m_MockApi.Verify(api => 
                api.Post(k_MockCloudProjectId, k_MockEnvironmentId, It.IsAny<JArray>()), Times.Never());
            m_MockApi.Verify(api => api.Put(k_MockCloudProjectId,
                    k_MockEnvironmentId,
                    "id",
                    It.IsAny<JArray>()),
                Times.Once());
        }
        
        [UnityTest]
        public IEnumerator Analytics_DeployTriggered() => AsyncTest.AsCoroutine(Analytics_DeployTriggeredAsync);
        async Task Analytics_DeployTriggeredAsync()
        {
            var config = new JObject();
            m_MockApi
                .Setup(api => api.Fetch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new FetchResult(config)));
            
            await m_DeploymentManager.DeployAsync(m_ConfigFiles);
            
            m_MockAnalytics
                .Verify(a => 
                    a.SendDeployedEvent(
                        It.IsAny<int>(), 
                        It.IsAny<IEnumerable<RemoteConfigFile>>()), 
                    Times.Once());
        }
    }
}
#endif
