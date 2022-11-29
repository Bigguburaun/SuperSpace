using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using UnityEditor;

namespace Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling
{
    class FileParseException : RemoteConfigDeploymentException
    {
        const string k_FormatExample = "{'entries': {}, 'types': {}}";
        RemoteConfigFile m_File;

        public override string Message => $"{StatusDescription} {StatusDetail}";
    

        public override string StatusDescription => 
            L10n.Tr("Unable To Parse");

        public override string StatusDetail => $"The file {m_File.Name} is not of proper format {k_FormatExample} where each type can be successfully mapped to an entry. See schema at 'https://ugs-config-schemas.unity3d.com/v1/remote-config.schema.json'";


        public FileParseException(RemoteConfigFile file)
        {
            m_File = file;
            AffectedFiles = new List<RemoteConfigFile> {file};
        }
    }
}
