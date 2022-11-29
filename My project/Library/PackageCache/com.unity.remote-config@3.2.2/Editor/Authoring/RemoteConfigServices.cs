using Unity.RemoteConfig.Editor.Authoring.Editor.Networking;
using Unity.Services.DeploymentApi.Editor;
using Unity.Services.RemoteConfig.Authoring.Editor.Analytics;
using Unity.Services.RemoteConfig.Authoring.Shared.DependencyInversion;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using Unity.Services.RemoteConfig.Authoring.Editor.Deployment;
using Unity.Services.RemoteConfig.Authoring.Editor.Formatting;
using Unity.Services.RemoteConfig.Authoring.Editor.Networking;
using UnityEditor;

namespace Unity.Services.RemoteConfig.Authoring.Editor
{
    class RemoteConfigServices : AbstractRuntimeServices<RemoteConfigServices>
    {
        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            Instance.Initialize(new ServiceCollection());
            var deploymentProvider = Instance.GetService<DeploymentProvider>();
            Deployments.Instance.DeploymentProviders.Add(deploymentProvider);
        }

        internal override void Register(ServiceCollection collection)
        {
            collection.RegisterStartupSingleton(Factories.Default<DeploymentProvider, RemoteConfigDeploymentProvider>);
            collection.Register(Factories.Default<Command<RemoteConfigFile>, DeployCommand>);
            collection.Register(Factories.Default<IFormatValidator, FormatValidator>);
            collection.Register(Factories.Default<IDeploymentManager, DeploymentManager>);
            collection.Register(Factories.Default<IWebApiClient, RcWebApiClient>);
            collection.Register(Factories.Default<IDeploymentInfo, DeploymentInfo>);
            collection.Register(Factories.Default<IRemoteConfigParser, RemoteConfigParser>);
            collection.Register(Factories.Default<IRemoteConfigValidator, RemoteConfigValidator>);
            collection.Register(Factories.Default<ConfigTypeDeriver>);
            collection.Register(Factories.Default<IAnalyticsWrapper, AnalyticsWrapper>);
            collection.Register(Factories.Default<IConfigAnalytics, ConfigAnalytics>);
        }
    }
}
