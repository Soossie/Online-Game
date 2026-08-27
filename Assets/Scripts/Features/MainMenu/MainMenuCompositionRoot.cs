using Application;
using Contracts.SceneControl;
using UnityEngine;

namespace Features
{
    public class MainMenuCompositionRoot : MonoBehaviour, ILoadedSceneCompositionRoot
    {
        public void Initialize(AppDependencies dependencies)
        {
            Debug.Log("Main menu loaded");
        }
    }
}
