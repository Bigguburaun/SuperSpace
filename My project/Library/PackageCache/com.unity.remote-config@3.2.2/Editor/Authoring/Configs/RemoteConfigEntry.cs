using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Configs
{
    class RemoteConfigEntry
    {
        [JsonProperty("key")]
        public string Key;
        [JsonProperty("value")]
        public JToken Value;
        [JsonProperty("type")]
        public string Type;
    }
}
