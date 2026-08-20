using System;
using Contracts.SceneControl;

namespace Application
{
    public readonly struct AppDependencies
    {
        public ISceneFlowController SceneFlowController { get; }
        public AppDependencies(ISceneFlowController sceneFlowController)
        {
            SceneFlowController = sceneFlowController ?? throw new ArgumentNullException(nameof(sceneFlowController));
        }
    }
}
