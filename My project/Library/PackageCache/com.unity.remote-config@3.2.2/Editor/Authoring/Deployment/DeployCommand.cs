using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.DeploymentApi.Editor;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using UnityEditor;
using UnityEngine;
using Logger = Unity.Services.RemoteConfig.Authoring.Shared.Logging.Logger;

namespace Unity.Services.RemoteConfig.Authoring.Editor.Deployment
{
    class DeployCommand : Command<RemoteConfigFile>
    {
        bool m_IsBusy;
        public override string Name => L10n.Tr("Deploy");

        IDeploymentManager m_DeploymentManager;

        public DeployCommand(IDeploymentManager deploymentManager)
        {
            m_DeploymentManager = deploymentManager;
        }

        public override async Task ExecuteAsync(IEnumerable<RemoteConfigFile> items, CancellationToken cancellationToken = default)
        {
            if (m_IsBusy) return;

            try
            {
                m_IsBusy = true;
                var remoteConfigFiles = items as IReadOnlyList<RemoteConfigFile> ?? items.ToList().AsReadOnly();
                Logger.LogVerbose($"Deployment triggered: {string.Join(", ", remoteConfigFiles.Select(item => item.Name))}");
                await m_DeploymentManager.DeployAsync(remoteConfigFiles);
            }
            finally
            {
                m_IsBusy = false;
            }
        }
    }
}
