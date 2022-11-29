using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Deployment
{
    interface IDeploymentManager
    {
        Task DeployAsync(IReadOnlyList<RemoteConfigFile> configFiles);
    }
}
