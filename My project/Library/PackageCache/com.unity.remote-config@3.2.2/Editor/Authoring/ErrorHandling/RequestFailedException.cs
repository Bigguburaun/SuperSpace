using System;
using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using Unity.Services.RemoteConfig.Authoring.Editor.ErrorHandling;
using UnityEditor;

namespace Unity.RemoteConfig.Editor.Authoring.Editor.Networking
{
    class RequestFailedException : RemoteConfigDeploymentException
    {
        long m_ErrorCode;
        string m_ErrorMessage;

        public override string Message => $"{StatusDescription} {StatusDetail}";
        public override string StatusDescription => $"Deployment Failed [{m_ErrorCode}].";
        public override string StatusDetail => m_ErrorMessage;
        
        public RequestFailedException(long errorCode, string errorMessage)
        {
            m_ErrorCode = errorCode;
            m_ErrorMessage = errorMessage;
            AffectedFiles = new List<RemoteConfigFile>();
        }
    }
}
