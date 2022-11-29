using System.Collections.Generic;
using NUnit.Framework;
using Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling;
using Unity.Services.RemoteConfig.Authoring.Editor.Formatting;
using Unity.Services.RemoteConfig.Tests.Editor.Authoring.Configs;
using UnityEngine;

namespace Unity.Services.RemoteConfig.Tests.Editor.Authoring.Formatting
{
    public class FormatValidatorTests
    {
        FormatValidator m_FormatValidator;
        
        [SetUp]
        public void SetUp()
        {
            m_FormatValidator = new FormatValidator();
        }

        [Test]
        public void ValidFormatWithTypesProvided_ValidationSucceeded()
        {
            var file = TestFileGetter.GetTestFile(AssetFilePaths.Valid1Key);
            var deploymentExceptions = new List<RemoteConfigDeploymentException>();
            
            m_FormatValidator.Validate(file, deploymentExceptions);
            
            Assert.IsTrue(deploymentExceptions.Count == 0);
        }
        
        [Test]
        public void ValidFormatWithoutTypesProvided_ValidationSucceeded()
        {
            var file = TestFileGetter.GetTestFile(AssetFilePaths.ValidNoTypes);
            var deploymentExceptions = new List<RemoteConfigDeploymentException>();
            m_FormatValidator.Validate(file, deploymentExceptions);
            
            Assert.IsTrue(deploymentExceptions.Count == 0);
        }

        [Test]
        public void MismatchingTypesProvided_ValidationFailed()
        {
            var file = TestFileGetter.GetTestFile(AssetFilePaths.InvalidMismatchInTypes);
            var deploymentExceptions = new List<RemoteConfigDeploymentException>();
            m_FormatValidator.Validate(file, deploymentExceptions);
            
            Assert.IsTrue(deploymentExceptions.Count == 1);
            Assert.IsTrue(deploymentExceptions[0] is MissingEntryForTypeException);
        }

        [Test]
        public void NoEntriesProvided_ValidationFailed()
        {
            var file = TestFileGetter.GetTestFile(AssetFilePaths.InvalidNoEntries);
            var deploymentExceptions = new List<RemoteConfigDeploymentException>();
            m_FormatValidator.Validate(file, deploymentExceptions);
            
            Assert.IsTrue(deploymentExceptions.Count == 1);
            Assert.IsTrue(deploymentExceptions[0] is NoEntriesException);
        }

        [Test]
        public void InvalidTypeProvided_ValidationFailed()
        {
            var file = TestFileGetter.GetTestFile(AssetFilePaths.InvalidTypeSpecified);
            var deploymentExceptions = new List<RemoteConfigDeploymentException>();
            m_FormatValidator.Validate(file, deploymentExceptions);
            
            Assert.IsTrue(deploymentExceptions.Count == 1);
            Assert.IsTrue(deploymentExceptions[0] is InvalidTypeException);
        }

        [Test]
        public void InvalidKeyPresent_ValidationFailed()
        {
            var file = TestFileGetter.GetTestFile(AssetFilePaths.InvalidKeyPresent);
            var deploymentExceptions = new List<RemoteConfigDeploymentException>();
            m_FormatValidator.Validate(file, deploymentExceptions);
            
            Assert.IsTrue(deploymentExceptions.Count == 1);
            Assert.IsTrue(deploymentExceptions[0] is FileParseException);
        }
    }
}
