using Application;
using UnityEngine;

namespace Contracts.SceneControl
{
    public interface ILoadedSceneCompositionRoot
    {
        void Initialize(AppDependencies dependencies);
    }
}
