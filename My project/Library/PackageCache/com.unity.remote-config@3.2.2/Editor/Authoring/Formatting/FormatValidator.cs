using System;
using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Formatting
{
    class FormatValidator : IFormatValidator
    {
        public bool Validate(
            RemoteConfigFile remoteConfigFile, 
            ICollection<RemoteConfigDeploymentException> deploymentExceptions)
        {
            if (remoteConfigFile.Content == null)
            {
                deploymentExceptions.Add(new FileParseException(remoteConfigFile));
                return false;
            }
            
            if (!IsProperFormat(remoteConfigFile, deploymentExceptions) 
                || !TypesCanBeMapped(remoteConfigFile, deploymentExceptions))
            {
                return false;
            }

            return true;
        }

        static bool IsProperFormat(
            RemoteConfigFile remoteConfigFile,
            ICollection<RemoteConfigDeploymentException> deploymentExceptions)
        {
            var canBeMapped = true;

            if (remoteConfigFile.Content.Entries == null
                || remoteConfigFile.Content.Entries.Count == 0)
            {
                deploymentExceptions.Add(new NoEntriesException(remoteConfigFile));
                canBeMapped = false;
            }

            return canBeMapped;
        }

        static bool TypesCanBeMapped(
            RemoteConfigFile remoteConfigFile, 
            ICollection<RemoteConfigDeploymentException> deploymentExceptions)
        {
            if (remoteConfigFile.Content.Types == null
                || remoteConfigFile.Content.Types.Count == 0)
            {
                return true;
            }

            var canBeMapped = true;
            var typesAreValid = true;

            foreach (var typeKvp in remoteConfigFile.Content.Types)
            {
                var typeKey = typeKvp.Key;
                var typeValue = typeKvp.Value;
                
                if (!remoteConfigFile.Content.Entries.ContainsKey(typeKey))
                {
                    deploymentExceptions.Add(new MissingEntryForTypeException(typeKey, remoteConfigFile));
                    canBeMapped = false;
                }

                if (!IsTypeValid(typeValue))
                {
                    deploymentExceptions.Add(new InvalidTypeException(remoteConfigFile, typeKey));
                    typesAreValid = false;
                }
            }

            return canBeMapped && typesAreValid;
        }

        static bool IsTypeValid(ConfigType type)
        {
            if (!Enum.IsDefined(typeof(ConfigType), type))
            {
                return false;
            }

            return true;
        }
    }
}
