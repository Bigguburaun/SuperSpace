namespace Unity.Services.RemoteConfig.Authoring.Editor.Deployment
{
    interface IDeploymentInfo
    {
        public string EnvironmentId { get; }
        public string CloudProjectId { get; }
    }
}
