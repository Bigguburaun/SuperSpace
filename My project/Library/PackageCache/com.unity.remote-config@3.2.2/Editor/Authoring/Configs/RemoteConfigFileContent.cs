using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Services.RemoteConfig.Authoring.Editor.Formatting;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Configs
{
    class RemoteConfigFileContent
    {
        [JsonProperty("$schema")]
        string m_Schema;
        
        public bool HasSchema => !string.IsNullOrEmpty(m_Schema);
        [JsonProperty("entries")]
        public Dictionary<string, JToken> Entries;
        [JsonProperty("types")]
        public Dictionary<string, ConfigType> Types;
    }
}
