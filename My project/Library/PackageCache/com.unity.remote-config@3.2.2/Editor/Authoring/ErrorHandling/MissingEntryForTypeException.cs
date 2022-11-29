using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using UnityEditor;

namespace Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling
{
    class MissingEntryForTypeException : RemoteConfigDeploymentException
    {
        RemoteConfigFile m_File;
        string m_Key;

        public override string Message => $"{StatusDescription} {StatusDetail}";
            
        public override string StatusDescription =>
            L10n.Tr($"Invalid Format.");
        public override string StatusDetail =>
            $"The key '{m_Key}' in the file '{m_File.Name}' was not found in the entries but exists in the types.";

        public MissingEntryForTypeException(string key, RemoteConfigFile file)
        {
            m_Key = key;
            m_File = file;
            AffectedFiles = new List<RemoteConfigFile> { file };
        }
    }
}
