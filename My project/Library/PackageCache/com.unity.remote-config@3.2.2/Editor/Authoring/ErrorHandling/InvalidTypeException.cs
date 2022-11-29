using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using UnityEditor;

namespace Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling
{
    class InvalidTypeException : RemoteConfigDeploymentException
    {
        string m_TypeName;
        RemoteConfigFile m_File;

        public override string Message => $"{StatusDescription} {StatusDetail}";

        public override string StatusDescription =>
            L10n.Tr("Invalid type specified");

        public override string StatusDetail =>
            $"{m_TypeName} specifies an invalid type. A type must be one of 'STRING`, `INT`, `BOOL`, `FLOAT`, `LONG`, `JSON`";

        public InvalidTypeException(RemoteConfigFile file, string typeName)
        {
            m_File = file;
            m_TypeName = typeName;
            AffectedFiles = new List<RemoteConfigFile> {file};
        }
    }
}
