using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace SceneManagement
{
    [CreateAssetMenu(fileName = "ScenePathSO", menuName = "Verkkopeli/ScenePathSO")]
    public sealed class ScenePathSO : ScriptableObject
    {
        // Makes path readonly outside this script
        public string Path => path;
        
        [Header("Scene setup")]
        [SerializeField, HideInInspector] private string path;
        
#if UNITY_EDITOR
        [SerializeField] private SceneAsset sceneAsset;
        
        private void OnValidate()
        {
            path = sceneAsset != null ? AssetDatabase.GetAssetPath(sceneAsset) : string.Empty;
        }
#endif
    }
}
