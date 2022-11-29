#if NUGET_MOQ_AVAILABLE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Unity.Services.DeploymentApi.Editor;
using Unity.Services.RemoteConfig.Authoring.Editor.Configs;
using Unity.Services.RemoteConfig.Authoring.Editor.Deployment;
using UnityEngine.TestTools;
using static Unity.Services.RemoteConfig.Tests.Editor.Authoring.Shared.AsyncTest;

namespace Unity.Services.RemoteConfig.Editor.Authoring.Core.Commands
{
    class DeployCommandTests
    {
        [UnityTest]
        public IEnumerator OnlyOneConcurrentDeploy() => AsCoroutine(OnlyOneConcurrentDeployAsync);

        static async Task OnlyOneConcurrentDeployAsync()
        {
            var mockDeploymentHandler = new Mock<IDeploymentManager>();
            mockDeploymentHandler
                .Setup(dh => dh.DeployAsync(It.IsAny<IReadOnlyList<RemoteConfigFile>>()))
                .Returns(() => Task.Delay(100));
            var deployCommand = new DeployCommand(mockDeploymentHandler.Object);


            var deploymentItems = new List<IDeploymentItem>();
            var task1 = deployCommand.ExecuteAsync(deploymentItems);
            var task2 = deployCommand.ExecuteAsync(deploymentItems);
            await Task.WhenAll(task1, task2);

            mockDeploymentHandler
                .Verify(
                    x => x.DeployAsync(It.IsAny<IReadOnlyList<RemoteConfigFile>>()), 
                    Times.Once());

            await deployCommand.ExecuteAsync(deploymentItems);

            mockDeploymentHandler
                .Verify(
                    x => x.DeployAsync(It.IsAny<IReadOnlyList<RemoteConfigFile>>()), 
                    Times.Exactly(2));
        }

    }
}
#endif
