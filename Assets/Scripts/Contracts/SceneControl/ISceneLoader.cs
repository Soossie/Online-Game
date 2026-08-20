using SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Contracts.SceneControl
{
    public interface ISceneLoader
    {
        public AsyncOperation LoadAdditivelyAsync(ScenePathSO pathSO);
        public AsyncOperation UnloadAsync(Scene scene);
        public void SetActiveScene(Scene scene);
        public bool TryGetLoadedScene(ScenePathSO pathSO, out Scene scene);
    }
}
