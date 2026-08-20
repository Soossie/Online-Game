using System;
using System.Collections;
using System.Collections.Generic;
using Application;
using Contracts.SceneControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class SceneFlowController: MonoBehaviour, ISceneFlowController, ISceneTransitionEvents
    {
        public event Action TransitionStarted;
        public event Action TransitionCompleted;
        public event Action<float> ProgressChanged;
        
        private AppDependencies? _appDependencies;
        private Scene _primaryScene; 
        private ISceneLoader _sceneLoader;
        private readonly List<Scene> _secondaryScenes = new();
        private bool _isTransitioning;

        public void Initialize(AppDependencies dependencies)
        {
            _appDependencies = dependencies;
        }

        public void Bind(ISceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader 
                           ?? throw new ArgumentNullException(nameof(sceneLoader));
        }
        
        // Change primary scene and unload secondary scenes
        public void ChangePrimaryScene(ScenePathSO pathSO)
        {
            if (_isTransitioning)
                return;
            _isTransitioning = true;
            StartCoroutine(ChangePrimarySceneCoroutine(pathSO));
        }

        // Add a secondary scene
        public void AddScene(ScenePathSO pathSO)
        {
           StartCoroutine(AddSceneCoroutine(pathSO));
        }

        // Remove a secondary scene
        public void RemoveScene(Scene scene)
        {
            StartCoroutine((RemoveSceneCoroutine(scene)));
        }

        // Remove a secondary scene with path
        public void RemoveScene(ScenePathSO pathSO)
        {
            if (!_sceneLoader.TryGetLoadedScene(pathSO, out Scene scene))
                return;

            RemoveScene(scene);
        }
        
        // Smooth loading bar
        private IEnumerator TrackProgressCoroutine(AsyncOperation operation, float startProgress, float endProgress)
        {
            while (!operation.isDone)
            {
                float operationProgress = Mathf.Clamp01(operation.progress);
                float transitionProg = Mathf.Lerp(startProgress, endProgress, operationProgress);
                ProgressChanged?.Invoke(transitionProg);
                yield return null;
            }
            ProgressChanged?.Invoke(endProgress);
        }
        
        // Change the primary scene and put it into secondary scenes
        private IEnumerator ChangePrimarySceneCoroutine(ScenePathSO pathSo)
        {
            TransitionStarted?.Invoke();
            ProgressChanged?.Invoke(0);
            
            Scene prevPrimaryScene = _primaryScene;
            List<Scene> prevSecondaryScreens = _secondaryScenes;

            AsyncOperation loadingOperation = _sceneLoader.LoadAdditivelyAsync(pathSo);
            yield return TrackProgressCoroutine(loadingOperation, 0, 0.8f);

            // If the scene path is invalid
            if (!_sceneLoader.TryGetLoadedScene(pathSo, out Scene scene))
            {
                _isTransitioning = false;
                TransitionCompleted?.Invoke();
                Debug.LogError($"Failed to load scene with path: {pathSo}");
                yield break;
            }
            
            _secondaryScenes.Clear();
            _sceneLoader.SetActiveScene(scene);
            InitializeScene(scene);
            _primaryScene = scene;
            
            foreach (var secondaryScene in _secondaryScenes)
            {
                // Unload secondary scenes
                if (secondaryScene.IsValid() && secondaryScene.isLoaded)
                    yield return _sceneLoader.UnloadAsync(secondaryScene);
            }

            if (prevPrimaryScene.IsValid() && prevPrimaryScene.isLoaded)
            {
                // Unload the previous primary scene
                AsyncOperation unloadingOperation = _sceneLoader.UnloadAsync(prevPrimaryScene);
                yield return TrackProgressCoroutine(unloadingOperation, 0.8f, 1f);
            }
            
            // Loading done
            ProgressChanged?.Invoke(1f);
            TransitionCompleted?.Invoke();
        }
        
        private IEnumerator AddSceneCoroutine(ScenePathSO pathSO)
        {
            AsyncOperation loadingOperation = _sceneLoader.LoadAdditivelyAsync(pathSO);
            yield return loadingOperation;

            if (!_sceneLoader.TryGetLoadedScene(pathSO, out Scene scene))
                yield break;
            
            InitializeScene(scene);
            _secondaryScenes.Add(scene);
        }

        private IEnumerator RemoveSceneCoroutine(Scene scene)
        {
            if (scene.IsValid() && scene.isLoaded) 
                yield return _sceneLoader.UnloadAsync(scene);
            
            _secondaryScenes.Remove(scene);
        }
        
        private void InitializeScene(Scene scene)
        {
            if (!_appDependencies.HasValue) 
                throw new InvalidOperationException(nameof(InitializeScene));
            GameObject[] rootObjects = scene.GetRootGameObjects();
            
            if (rootObjects.Length != 1 || !rootObjects[0].TryGetComponent(out ILoadedSceneCompositionRoot root))
                throw new InvalidOperationException(nameof(InitializeScene));
            
            root.Initialize(_appDependencies.Value);
        }
    }
}
