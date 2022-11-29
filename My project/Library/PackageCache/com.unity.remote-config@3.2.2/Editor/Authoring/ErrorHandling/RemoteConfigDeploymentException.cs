using System;
using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;

namespace Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling
{
    abstract class RemoteConfigDeploymentException : Exception
    {
        public List<RemoteConfigFile> AffectedFiles { get; protected set; }
        public abstract string StatusDescription { get; }
        public abstract string StatusDetail { get; }
    }
}
