using SceneManagement;

namespace Application
{
    public readonly struct StartupScenes
    {
        public ScenePathSO LoginSceneSO { get; }
        public ScenePathSO MainMenuSO { get; }

        public StartupScenes(ScenePathSO loginSO, ScenePathSO menuSO)
        {
            LoginSceneSO = loginSO;
            MainMenuSO = menuSO;
        }
    }
}