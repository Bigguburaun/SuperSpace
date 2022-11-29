using System.Collections.Generic;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Configs
{
    interface IRemoteConfigParser
    {
        Dictionary<RemoteConfigEntry, RemoteConfigFile> ParseFiles(IEnumerable<RemoteConfigFile> file);
    }
}
