using System.Collections.Generic;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Analytics
{
    interface IConfigAnalytics
    {
        void SendCreatedEvent();
        void SendDeployedEvent(int totalFilesRequested, IEnumerable<RemoteConfigFile> validFiles);
    }
}
