using System;
using System.IO;
using Contracts.Netcode;
using Netcode;
using Netcode.Authentication;
using SceneManagement;
using UnityEngine;

namespace Application
{
    public class BootStrapCompositionRoot: MonoBehaviour
    {
        [SerializeField] private SceneFlowController sceneFlowController;
        [SerializeField] private LoadingScreenController loadingScreenController;
        [SerializeField] private ScenePathSO mainMenuSceneSo;
        [SerializeField] private ScenePathSO loginSceneSo;
        [SerializeField] private NetworkSettingsSO settingsSo;

        private GameApp _app;

        private void Awake()
        {
            if (!sceneFlowController)
                throw new ArgumentNullException(nameof(sceneFlowController));
            
            if (!loadingScreenController)
                throw new ArgumentNullException(nameof(loadingScreenController));
            
            if (!mainMenuSceneSo)
                throw new ArgumentNullException(nameof(mainMenuSceneSo));
            
            if (!loginSceneSo)
                throw new ArgumentNullException(nameof(loginSceneSo));
            
            string authPath = Path.Combine(
                UnityEngine.Application.persistentDataPath, 
                AppConstants.Authentication.DataPath);
            
            NodeClient nodeClient = new(settingsSo.BaseUrl);

            IAuthenticationClient authClient = nodeClient;
            IPlayerProfileClient profileClient = nodeClient;
            IRefreshAuthentication refreshAuth = nodeClient;
            IAuthenticationSessionStore authSessionStore = new JsonAuthenticationSessionStore(authPath);
            
            // Adds a sceneloader, sceneflowcontroller and builds in a single step using .Add helpers
            GameAppBuilder appBuilder = new();
            _app = appBuilder
                .Add(new SceneLoader())
                .Add(sceneFlowController)
                .Add(loadingScreenController)
                .Add(new StartupScenes(menuSO: mainMenuSceneSo, loginSO: loginSceneSo))
                .Add(authClient)
                .Add(profileClient)
                .Add(authSessionStore)
                .Add(refreshAuth)
                .Build();

            Debug.Log($"Authentication session path: {authPath}");
        }

        private async Awaitable Start()
        {
            if (_app is null)
                throw new ArgumentNullException(nameof(_app));
            
            await _app.StartAsync(destroyCancellationToken);
        }
    }
}
