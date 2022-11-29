using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Unity.Services.RemoteConfig.Authoring.Editor.Formatting;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Configs
{
    class RemoteConfigParser : IRemoteConfigParser
    {
        ConfigTypeDeriver m_ConfigTypeDeriver;

        public RemoteConfigParser(ConfigTypeDeriver configTypeDeriver)
        {
            m_ConfigTypeDeriver = configTypeDeriver;
        }

        public Dictionary<RemoteConfigEntry, RemoteConfigFile> ParseFiles(IEnumerable<RemoteConfigFile> configFiles)
        {
            var entryToFileMap = new Dictionary<RemoteConfigEntry, RemoteConfigFile>();
            foreach (var remoteConfigFile in configFiles)
            {
                var entries = ParseFile(remoteConfigFile);
                entries.ForEach(entry => entryToFileMap.Add(entry, remoteConfigFile));
            }

            return entryToFileMap;
        }

        public List<RemoteConfigEntry> ParseFile(RemoteConfigFile remoteConfigFile)
        {
            var remoteConfigEntries = new List<RemoteConfigEntry>();

            foreach (var entry in remoteConfigFile.Content.Entries)
            {
                var type = m_ConfigTypeDeriver.DeriveType(entry.Value);
                if (remoteConfigFile.Content.Types != null && remoteConfigFile.Content.Types.ContainsKey(entry.Key))
                {
                    type = remoteConfigFile.Content.Types[entry.Key];
                }

                var remoteConfigEntry = new RemoteConfigEntry()
                {
                    Key = entry.Key,
                    Type = type.ToString().ToLower(),
                    Value = entry.Value
                };
                remoteConfigEntries.Add(remoteConfigEntry);
            }
            
            return remoteConfigEntries;
        }
    }
}
