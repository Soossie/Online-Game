using Contracts.SceneControl;
using SceneManagement;
using UnityEngine;
namespace Application
{
    public static class DependencyRegistrations
    {
        public static GameAppBuilder Add(this GameAppBuilder builder, ISceneLoader sceneLoader)
        {
            builder.RegisterSceneLoader(sceneLoader);
            return builder;
        }

        public static GameAppBuilder Add(this GameAppBuilder builder, SceneFlowController sceneFlowController)
        {
            builder.RegisterSceneFlowController(sceneFlowController);
            return builder;
        }
        
        public static GameAppBuilder Add(this GameAppBuilder builder, LoadingScreenController loadingScreenController)
        {
            builder.RegisterLoadingScreenController(loadingScreenController);
            return builder;
        }
        
        public static GameAppBuilder Add(this GameAppBuilder builder, ScenePathSO pathSo)
        {
            builder.RegisterPathSO(pathSo);
            return builder;
        }
    }
}
