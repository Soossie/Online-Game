using System;
using Contracts.SceneControl;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SceneManagement
{
    public class SceneNavAction : MonoBehaviour, ISceneNavActions
    {
        [SerializeField] private ScenePathSO destinationPathSO;
        private ISceneFlowController _sceneFlowController;
        public void Bind(ISceneFlowController sceneFlowController)
        {
            _sceneFlowController = sceneFlowController 
                                   ?? throw new ArgumentNullException(nameof(sceneFlowController));
        }
        
        // ?. is a nullcheck before executing
        public void ChangeToScene()
        {
            _sceneFlowController?.ChangePrimaryScene(destinationPathSO);
        }

        public void AddScene()
        {
            _sceneFlowController?.AddScene(destinationPathSO);
        }
        
        public void RemoveScene()
        {
            _sceneFlowController?.RemoveScene(destinationPathSO);
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            UnityEngine.Application.Quit();
        }
    }
}
