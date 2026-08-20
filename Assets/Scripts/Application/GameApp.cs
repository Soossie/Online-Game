using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.SceneControl;
using SceneManagement;

namespace Application
{
    public sealed class GameApp
    {
        private readonly ISceneFlowController _sceneFlowController;
        private ScenePathSO _pathSo;
        
        private bool _hasStarted;

        public GameApp(ISceneFlowController sceneFlowController, ScenePathSO pathso)
        {
            _sceneFlowController = sceneFlowController 
                                   ?? throw new ArgumentNullException(nameof(sceneFlowController));
            _pathSo = pathso ??
                throw new ArgumentNullException(nameof(pathso));
        }

        public Task StartAsync(CancellationToken ctx = default)
        {
            if (_hasStarted)
                return Task.CompletedTask;
            
            _hasStarted = true;
            ctx.ThrowIfCancellationRequested();
            
            _sceneFlowController.ChangePrimaryScene(_pathSo);
            
            return Task.CompletedTask;
        }
    }
}
