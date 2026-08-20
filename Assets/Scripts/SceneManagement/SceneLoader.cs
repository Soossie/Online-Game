using Contracts.SceneControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public sealed class SceneLoader : ISceneLoader
    {
        public AsyncOperation LoadAdditivelyAsync(ScenePathSO pathSO)
        {
            return SceneManager.LoadSceneAsync(pathSO.Path, LoadSceneMode.Additive);
        }

        public AsyncOperation UnloadAsync(Scene scene)
        {
            return SceneManager.UnloadSceneAsync(scene);
        }

        public void SetActiveScene(Scene scene)
        {
            SceneManager.SetActiveScene(scene);
        }

        public bool TryGetLoadedScene(ScenePathSO pathSO, out Scene scene)
        {
            scene = SceneManager.GetSceneByPath(pathSO.Path);
            return scene.IsValid() && scene.isLoaded;
        }
    }
}
