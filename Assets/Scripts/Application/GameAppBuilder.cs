using System;
using Contracts.Netcode;
using Contracts.SceneControl;
using Netcode.Authentication;
using Profiles;
using SceneManagement;

namespace Application
{
    public sealed class GameAppBuilder
    {
        private SceneFlowController _sceneFlowController;
        private ISceneLoader _sceneLoader;
        private LoadingScreenController _loadingScreenController;
        private IAuthenticationClient _authClient;
        private IRefreshAuthentication _refreshAuth;
        private IAuthenticationSessionStore _authSessionStore;
        private IPlayerProfileClient _profileClient;
        private StartupScenes _startupScenes;
        
        public void RegisterAuthenticationClient(IAuthenticationClient authClient)
        {
            if (_authClient is not null)
                throw new InvalidOperationException(nameof(RegisterAuthenticationClient));

            _authClient = authClient;
        }
        
        public void RegisterAuthenticationSessionStore(IAuthenticationSessionStore authSessionStore)
        {
            if (_authSessionStore is not null)
                throw new InvalidOperationException(nameof(RegisterAuthenticationSessionStore));

            _authSessionStore = authSessionStore;
        }

        public void RegisterPlayerProfileClient(IPlayerProfileClient profileClient)
        {
            if (_profileClient is not null)
                throw new InvalidOperationException(nameof(RegisterPlayerProfileClient));

            _profileClient = profileClient;
        }
        
        public void RegisterRefreshAuthentication(IRefreshAuthentication refreshAuth)
        {
            if (_refreshAuth is not null)
                throw new InvalidOperationException(nameof(RegisterRefreshAuthentication));

            _refreshAuth = refreshAuth;
        }

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
        
        public void RegisterStartupScenes(StartupScenes scenes)
        {
            if (!scenes.LoginSceneSO || !scenes.MainMenuSO)
                throw new ArgumentNullException(nameof(RegisterStartupScenes));
            
            _startupScenes = scenes;
        }

        public GameApp Build()
        {
            if (!HasValidRefs())
                throw new InvalidOperationException(nameof(Build));

            PlayerProfileService profileService = new();
            AuthenticationService authService = new(
                _authClient,
                _refreshAuth,
                _profileClient,
                _authSessionStore,
                profileService
                );

            _sceneFlowController.Bind(_sceneLoader);
            _loadingScreenController.Bind(_sceneFlowController);
            AppDependencies appDependencies = new(_sceneFlowController, authService, profileService);
            _sceneFlowController.Initialize(appDependencies);
            
            return new GameApp(_sceneFlowController, authService, _startupScenes);
        }

        private bool HasValidRefs()
        {
            return _sceneFlowController != null
                   && _sceneLoader != null
                   && _loadingScreenController != null
                   && _authClient != null
                   && _profileClient != null
                   && _refreshAuth != null
                   && _authSessionStore != null;
        }
    }
}
