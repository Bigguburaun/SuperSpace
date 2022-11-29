using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Formatting
{
    interface IFormatValidator
    {
        bool Validate(
            RemoteConfigFile remoteConfigFiles,
            ICollection<RemoteConfigDeploymentException> deploymentExceptions);
    }
}
