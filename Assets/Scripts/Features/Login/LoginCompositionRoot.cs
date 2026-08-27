using System;
using Application;
using Contracts.SceneControl;
using UnityEngine;

namespace Features.Login
{
    public class LoginCompositionRoot : MonoBehaviour, ILoadedSceneCompositionRoot
    {
        [SerializeField] private LoginController loginController;
        public void Initialize(AppDependencies dependencies)
        {
            if (!loginController)
                throw new ArgumentNullException(nameof(loginController));
            Debug.Log("Login menu loaded");

            loginController.Bind(dependencies.SceneFlowController, dependencies.AuthenticationService);
        }
    }
}
