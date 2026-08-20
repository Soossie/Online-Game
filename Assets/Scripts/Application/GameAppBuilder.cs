using System;
using Contracts.SceneControl;
using SceneManagement;

namespace Application
{
    public sealed class GameAppBuilder
    {
        private SceneFlowController _sceneFlowController;
        private ISceneLoader _sceneLoader;
        private LoadingScreenController _loadingScreenController;
        private ScenePathSO _pathSo;

        public void RegisterSceneFlowController(SceneFlowController sceneFlowController)
        {
            if (!sceneFlowController)
                throw new InvalidOperationException(nameof(RegisterSceneFlowController));

            _sceneFlowController = sceneFlowController;
        }

        public void RegisterSceneLoader(ISceneLoader sceneLoader)
        {
            if (_sceneLoader is not null)
                throw new InvalidOperationException(nameof(RegisterSceneLoader));
            
            _sceneLoader = sceneLoader;
        }
        
        public void RegisterLoadingScreenController(LoadingScreenController loadingScreenController)
        {
            if (!loadingScreenController)
                throw new InvalidOperationException(nameof(RegisterLoadingScreenController));

            _loadingScreenController = loadingScreenController;
        }
        
        public void RegisterPathSO(ScenePathSO pathSo)
        {
            if (_pathSo)
                throw new InvalidOperationException(nameof(RegisterPathSO));

            _pathSo = pathSo;
        }

        public GameApp Build()
        {
            if (!HasValidRefs())
                throw new InvalidOperationException(nameof(Build));

            _sceneFlowController.Bind(_sceneLoader);
            _loadingScreenController.Bind(_sceneFlowController);
            AppDependencies appDependencies = new(_sceneFlowController);
            _sceneFlowController.Initialize(appDependencies);
            
            return new GameApp(_sceneFlowController, _pathSo);
        }

        private bool HasValidRefs()
        {
            return _sceneFlowController != null 
                   && _sceneLoader != null
                   && _loadingScreenController != null
                   && _pathSo != null;
        }
    }
}
