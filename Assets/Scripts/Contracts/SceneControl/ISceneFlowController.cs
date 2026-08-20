using SceneManagement;

namespace Contracts.SceneControl
{
    public interface ISceneFlowController
    {
        public void ChangePrimaryScene(ScenePathSO pathSO);
        public void AddScene(ScenePathSO pathSO);
        public void RemoveScene(ScenePathSO pathSO);
    }
}
