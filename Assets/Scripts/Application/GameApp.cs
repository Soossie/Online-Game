using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Netcode;
using Contracts.SceneControl;
using SceneManagement;
using UnityEngine;

namespace Application
{
    public sealed class GameApp
    {
        private readonly ISceneFlowController _sceneFlowController;
        private readonly StartupScenes _startupScenes;
        private readonly IAuthenticationService _authService;
        
        private bool _hasStarted;

        public GameApp(
            ISceneFlowController sceneFlowController, 
            IAuthenticationService authService, 
            StartupScenes startupScenes)
        {
            _sceneFlowController = sceneFlowController 
                                   ?? throw new ArgumentNullException(nameof(sceneFlowController));
            /* TODO make nullable
            _scenes = scenes ??
                throw new ArgumentNullException(nameof(scenes));
                */
            _authService = authService ??
                throw new ArgumentNullException(nameof(authService));
            _startupScenes = startupScenes;
        }
        
        public async Task StartAsync(CancellationToken ctx = default)
        {
            if (_hasStarted)
                return;
            Debug.Log("Starting game app");
            _hasStarted = true;
            bool sessionRestored = await _authService.TryRestoreSessionASync(ctx);
            ctx.ThrowIfCancellationRequested();
            ScenePathSO startScene = sessionRestored 
                ? _startupScenes.MainMenuSO
                : _startupScenes.LoginSceneSO;
            Debug.Log("Starting scene: " + startScene.name);
            _sceneFlowController.ChangePrimaryScene(startScene);
            
        }
    }
}
