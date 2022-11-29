using System;
using System.IO;
using Newtonsoft.Json;
using Unity.Services.DeploymentApi.Editor;
using Unity.Services.RemoteConfig.Authoring.Shared.Threading;
using UnityEngine;

using PathIO = System.IO.Path;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Configs
{
    [Serializable]
    class RemoteConfigFile : DeploymentItem, ISerializationCallbackReceiver
    {
        RemoteConfigFileContent m_Content;

        public sealed override string Path
        {
            get => base.Path;
            set
            {
                base.Path = value;
                Name = System.IO.Path.GetFileName(value);
            }
        }

        public RemoteConfigFileContent Content
        {
            get
            {
                if (m_Content == null)
                {
                    TrySetContentFromPath(Path);
                }

                return m_Content;
            }
        }

        internal bool TrySetContentFromPath(string path)
        {
            m_Content = null;
            
            try
            {
                var settings = new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Error
                };
                m_Content = 
                    JsonConvert.DeserializeObject<RemoteConfigFileContent>(
                        File.ReadAllText(PathIO.GetFullPath(path)), settings);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void OnBeforeSerialize()
        {
            //do nothing
        }

        public void OnAfterDeserialize()
        {
            Sync.RunNextUpdateOnMain(() =>
            {
                if (Path != null)
                {
                    Name = System.IO.Path.GetFileName(Path);
                }
            });
        }
    }
}
