using System;
using Application;
using Contracts.SceneControl;
using UnityEngine;

namespace Features.MainMenu
{
    public class MainMenuCompositionRoot : MonoBehaviour, ILoadedSceneCompositionRoot
    {
        [SerializeField] private MainMenuController mainMenuController;
        public void Initialize(AppDependencies dependencies)
        {
            if (!mainMenuController)
                throw new ArgumentNullException(nameof(mainMenuController));
            Debug.Log("Main menu loaded");
            
            mainMenuController.Bind(dependencies.SceneFlowController, dependencies.PlayerProfileContext);
            mainMenuController.Greet();
        }
    }
}
