using Unity.Services.DeploymentApi.Editor;
using UnityEditor;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Deployment
{
    class DeploymentInfo : IDeploymentInfo
    {
        public string EnvironmentId => Deployments.Instance.EnvironmentProvider.Current;
        public string CloudProjectId => CloudProjectSettings.projectId;

        public override string ToString()
        {
            return $"EnvironmentId: {EnvironmentId}, CloudProjectId: {CloudProjectId}";
        }
    }
}
