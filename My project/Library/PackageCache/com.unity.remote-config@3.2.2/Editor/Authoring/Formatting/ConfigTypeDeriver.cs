using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Formatting
{
    enum ConfigType
    {
        STRING,
        INT,
        BOOL,
        FLOAT,
        LONG,
        JSON
    }

    class ConfigTypeDeriver
    {
        static Dictionary<Type, ConfigType> s_TypeToConfigType = new Dictionary<Type, ConfigType>()
        {
            { typeof(string), ConfigType.STRING },
            { typeof(int), ConfigType.INT },
            { typeof(bool), ConfigType.BOOL },
            { typeof(float), ConfigType.FLOAT },
            { typeof(double), ConfigType.FLOAT },
            { typeof(long), ConfigType.LONG },
            { typeof(JObject), ConfigType.JSON}
        };

        public ConfigType DeriveType(JToken config)
        {
            var value = config is JValue jvalue ? jvalue.Value : config as JObject;
            return s_TypeToConfigType[value.GetType()];
        }
    }
}