using System;
using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using UnityEditor;

namespace Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling
{
    class DuplicateKeysInMultipleFilesException : RemoteConfigDeploymentException
    {
        string m_Key;
        RemoteConfigFile m_File1;
        RemoteConfigFile m_File2;
        
        public override string Message => $"{StatusDescription} {StatusDetail}";
        
        public override string StatusDescription =>
            L10n.Tr($"Duplicate keys in files.");

        public override string StatusDetail
        {
            get
            {
                 var detail = $"Key '{m_Key}' was found in multiple files: ";
                 
                 foreach (var file in AffectedFiles)
                 {
                     detail += $" '{file.Name}'";
                 }

                 return detail;
            }     
        }
        

        public DuplicateKeysInMultipleFilesException(string key, IEnumerable<RemoteConfigFile> files)
        {
            m_Key = key;

            AffectedFiles = new List<RemoteConfigFile>(files);
        }
    }
}
