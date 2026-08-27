using System;
using Contracts.Netcode;
using Contracts.Profiles;
using Contracts.SceneControl;

namespace Application
{
    public readonly struct AppDependencies
    {
        public ISceneFlowController SceneFlowController { get; }
        public IAuthenticationService AuthenticationService { get; }
        public IPlayerProfileContext PlayerProfileContext { get; }
        
        public AppDependencies(ISceneFlowController sceneFlowController, 
            IAuthenticationService authenticationService, 
            IPlayerProfileContext playerProfileContext)
        {
            SceneFlowController = sceneFlowController 
                                  ?? throw new ArgumentNullException(nameof(sceneFlowController));
            AuthenticationService = authenticationService 
                                    ?? throw new ArgumentNullException(nameof(authenticationService));
            PlayerProfileContext = playerProfileContext 
                                   ?? throw new ArgumentNullException(nameof(playerProfileContext));
        }
    }
}
