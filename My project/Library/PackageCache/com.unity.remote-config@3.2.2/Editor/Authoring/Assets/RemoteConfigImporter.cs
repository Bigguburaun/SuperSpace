using System.IO;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Assets
{
    [RemoteConfigImporter]
    public class RemoteConfigImporter : ScriptedImporter
    {
        const string k_RemoteConfigAssetIdentifier = "RemoteConfig";
        const string k_SourceAssetIdentifier = "source";

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var remoteConfigAsset = ScriptableObject.CreateInstance<RemoteConfigAsset>();
            remoteConfigAsset.Model = new RemoteConfigFile()
            {
                Path = Path.Join(
                    Application.dataPath.Replace("Assets", ""),
                    ctx.assetPath)
            };

            var body = File.ReadAllText(remoteConfigAsset.Path);
            var bodyAsset = new TextAsset(body);
            ctx.AddObjectToAsset(k_RemoteConfigAssetIdentifier, remoteConfigAsset);
            ctx.AddObjectToAsset(k_SourceAssetIdentifier, bodyAsset);
            ctx.SetMainObject(bodyAsset);
        }
    }
}
