using System;

namespace Contracts.SceneControl
{
    public interface ISceneTransitionEvents
    {
        public event Action TransitionStarted;
        public event Action TransitionCompleted;
        public event Action<float> ProgressChanged;
    }
}
