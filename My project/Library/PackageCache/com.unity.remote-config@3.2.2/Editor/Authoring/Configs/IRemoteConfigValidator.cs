using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Configs
{
    interface IRemoteConfigValidator
    {
        bool Validate(
            Dictionary<RemoteConfigEntry, RemoteConfigFile> entryToFileMap,
            ICollection<RemoteConfigDeploymentException> deploymentExceptions);
    }
}
