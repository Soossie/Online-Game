using System;
using SceneManagement;
using UnityEngine;

namespace Application
{
    public class BootStrapCompositionRoot: MonoBehaviour
    {
        [SerializeField] private SceneFlowController sceneFlowController;
        [SerializeField] private LoadingScreenController loadingScreenController;
        [SerializeField] private ScenePathSO pathSO;

        private GameApp _app;

        private void Awake()
        {
            if (!sceneFlowController)
                throw new ArgumentNullException(nameof(sceneFlowController));
            
            if (!loadingScreenController)
                throw new ArgumentNullException(nameof(loadingScreenController));
            
            if (!pathSO)
                throw new ArgumentNullException(nameof(pathSO));
            
            // Adds a sceneloader, sceneflowcontroller and builds in a single step using .Add helpers
            GameAppBuilder appBuilder = new();
            _app = appBuilder
                .Add(new SceneLoader())
                .Add(sceneFlowController)
                .Add(loadingScreenController)
                .Add(pathSO)
                .Build();
        }

        private async Awaitable Start()
        {
            if (_app is null)
                throw new ArgumentNullException(nameof(_app));
            
            await _app.StartAsync(destroyCancellationToken);
        }
    }
}
