using Contracts.SceneControl;
using UnityEngine;
using UnityEngine.UI;

namespace SceneManagement
{
    public class LoadingScreenController : MonoBehaviour
    {
        [SerializeField] private GameObject loadingCanvas;
        [SerializeField] private Slider loadingBarSlider;
        private ISceneTransitionEvents _sceneTransitionEvents;

        public void Bind(ISceneTransitionEvents sceneTransitionEvents)
        {
            // Check if the object is the exact same in memory
            if (ReferenceEquals(_sceneTransitionEvents, sceneTransitionEvents)) return;
            Unbind();
            _sceneTransitionEvents = sceneTransitionEvents;
            
            if (_sceneTransitionEvents is null) return;
            
            // in c# 14 can use _sceneTransitionEvents?.TransitionStarted
            _sceneTransitionEvents.TransitionStarted += EnableCanvas;
            _sceneTransitionEvents.TransitionCompleted += DisableCanvas;
            _sceneTransitionEvents.ProgressChanged += UpdateLoadingProgressVisual;
        }

        public void Unbind()
        {
            if (_sceneTransitionEvents is null) return;
            
            _sceneTransitionEvents.TransitionStarted -= EnableCanvas;
            _sceneTransitionEvents.TransitionCompleted -= DisableCanvas;
            _sceneTransitionEvents.ProgressChanged -= UpdateLoadingProgressVisual;
        }

        private void EnableCanvas()
        {
            loadingCanvas.SetActive(true);
        }

        private void DisableCanvas()
        {
            loadingCanvas.SetActive(false);
        }

        private void UpdateLoadingProgressVisual(float currentProgress)
        {
            loadingBarSlider.value = currentProgress;
        }
    }
}
