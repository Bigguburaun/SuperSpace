using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling;
using Unity.Services.RemoteConfig.Authoring.Editor.Formatting;

namespace Unity.Services.RemoteConfig.Tests.Editor.Authoring.Configs
{
    public class RemoteConfigValidatorTests
    {
        RemoteConfigValidator m_RemoteConfigValidator = new RemoteConfigValidator();
        RemoteConfigParser m_Parser = new RemoteConfigParser(new ConfigTypeDeriver());

        [Test]
        public void NoDuplicatesSingleFile_ValidationSucceeds()
        {
            var file = TestFileGetter.GetTestFile(AssetFilePaths.ValidBase);
            var entries = m_Parser.ParseFile(file);
            var entryToFileMap = entries.ToDictionary(entry => entry, _ => file);

            var deploymentExceptions = new List<RemoteConfigDeploymentException>();
            m_RemoteConfigValidator.Validate(entryToFileMap, deploymentExceptions);

            Assert.IsTrue(deploymentExceptions.Count == 0);
        }

        [Test]
        public void NoDuplicatesMultipleFiles_ValidationSucceeds()
        {
            var file1 = TestFileGetter.GetTestFile(AssetFilePaths.ValidBase);
            var entries1 = m_Parser.ParseFile(file1);
            var entryToFileMap = entries1.ToDictionary(entry => entry, _ => file1);

            var file2 = TestFileGetter.GetTestFile(AssetFilePaths.ValidOther);
            var entries2 = m_Parser.ParseFile(file2);
            foreach (var entry in entries2)
            {
                entryToFileMap.Add(entry, file2);
            }

            var deploymentExceptions = new List<RemoteConfigDeploymentException>();
            m_RemoteConfigValidator.Validate(entryToFileMap, deploymentExceptions);

            Assert.IsTrue(deploymentExceptions.Count == 0);
        }
        
        [Test]
        public void DuplicateMultipleFiles_ValidationFails()
        {
            var file1 = TestFileGetter.GetTestFile(AssetFilePaths.ValidBase);
            var entries1 = m_Parser.ParseFile(file1);
            var entryToFileMap = entries1.ToDictionary(entry => entry, _ => file1);

            var file2 = TestFileGetter.GetTestFile(AssetFilePaths.ValidCopy);
            var entries2 = m_Parser.ParseFile(file2);
            foreach (var entry in entries2)
            {
                entryToFileMap.Add(entry, file2);
            }

            var deploymentExceptions = new List<RemoteConfigDeploymentException>();
            m_RemoteConfigValidator.Validate(entryToFileMap, deploymentExceptions);

            Assert.IsTrue(deploymentExceptions.Count == 2);
            Assert.IsTrue(deploymentExceptions[0] is DuplicateKeysInMultipleFilesException);
            Assert.IsTrue(deploymentExceptions[1] is DuplicateKeysInMultipleFilesException);
        }

        JArray GenerateJArray(string prefix, int size, bool duplicates = false)
        {
            var jarray = new JArray();
            
            for (var i = 0; i < size; i++)
            {
                var token = new JObject()
                {
                    {"key", $"{prefix}_key_{i}"},
                    {"type", "string"},
                    {"value", $"{prefix}_value"}
                };
                
                jarray.Add(token);

                if (duplicates)
                {
                    jarray.Add(token.DeepClone());
                }
            }

            return jarray;
        }
    }
}
