using Unity.Services.RemoteConfig.Authoring.Shared.Assets;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using UnityEngine;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Assets
{
    class RemoteConfigAsset : ScriptableObject, IPath
    {
        [SerializeField]
        RemoteConfigFile m_Model;

        public RemoteConfigFile Model
        {
            get => m_Model;
            internal set => m_Model = value;
        }

        public string Path
        {
            get => m_Model.Path;
            set => m_Model.Path = value;
        }
    }
}
