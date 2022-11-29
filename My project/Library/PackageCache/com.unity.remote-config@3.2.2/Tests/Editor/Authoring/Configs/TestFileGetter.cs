using System.IO;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;

namespace Unity.Services.RemoteConfig.Tests.Editor.Authoring.Configs
{
    static class TestFileGetter
    {
        public static RemoteConfigFile GetTestFile(string filePath)
        {
            var file = new RemoteConfigFile()
            {
                Path = GetTestFilePath(filePath)
            };
            return file;
        }
        
        static string GetTestFilePath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                filePath = filePath.Replace("com.unity.remote-config", "com.unity.remote-config.tests");
            }

            return filePath;
        }
    }
}
