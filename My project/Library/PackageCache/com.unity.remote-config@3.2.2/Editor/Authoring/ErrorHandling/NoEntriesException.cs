using System;
using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using UnityEditor;

namespace Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling
{
    class NoEntriesException : RemoteConfigDeploymentException
    {
        RemoteConfigFile m_File;

        public override string Message => $"{StatusDescription} {StatusDetail}";

        public override string StatusDescription =>
            L10n.Tr($"Invalid Format.");
        public override string StatusDetail => $"The file {m_File.Name} does not have a key 'entries'.";

        public NoEntriesException(RemoteConfigFile file)
        {
            m_File = file;
            AffectedFiles = new List<RemoteConfigFile> { file };
        }
    }
}
