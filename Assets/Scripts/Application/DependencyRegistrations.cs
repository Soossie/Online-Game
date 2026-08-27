using Contracts.Netcode;
using Contracts.SceneControl;
using SceneManagement;
using UnityEngine;
namespace Application
{
    public static class DependencyRegistrations
    {
        
        // TODO make these generic
        public static GameAppBuilder Add(
            this GameAppBuilder builder, 
            ISceneLoader sceneLoader)
        {
            builder.RegisterSceneLoader(sceneLoader);
            return builder;
        }

        public static GameAppBuilder Add(
            this GameAppBuilder builder, 
            SceneFlowController sceneFlowController)
        {
            builder.RegisterSceneFlowController(sceneFlowController);
            return builder;
        }
        
        public static GameAppBuilder Add(
            this GameAppBuilder builder, 
            LoadingScreenController loadingScreenController)
        {
            builder.RegisterLoadingScreenController(loadingScreenController);
            return builder;
        }
        
        public static GameAppBuilder Add(
            this GameAppBuilder builder, 
            StartupScenes scenes)
        {
            builder.RegisterStartupScenes(scenes);
            return builder;
        }

        public static GameAppBuilder Add(
            this GameAppBuilder builder,
            IPlayerProfileClient playerProfileClient)
        {
            builder.RegisterPlayerProfileClient(playerProfileClient);
            return builder;
        }

        public static GameAppBuilder Add(
            this GameAppBuilder builder,
            IAuthenticationSessionStore authSessionStore)
        {
            builder.RegisterAuthenticationSessionStore(authSessionStore);
            return builder;
        }
        
        public static GameAppBuilder Add(
            this GameAppBuilder builder,
            IAuthenticationClient authClient)
        {
            builder.RegisterAuthenticationClient(authClient);
            return builder;
        }
        
        public static GameAppBuilder Add(
            this GameAppBuilder builder,
            IRefreshAuthentication refreshAuth)
        {
            builder.RegisterRefreshAuthentication(refreshAuth);
            return builder;
        }
    }
}
