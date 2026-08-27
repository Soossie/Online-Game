using UnityEngine;

namespace Netcode
{
    [CreateAssetMenu(fileName = "NetworkSettingsSO", menuName = "Verkkopeli/NetworkSettingsSO")]
    public class NetworkSettingsSO: ScriptableObject
    {
        [SerializeField] private string _baseUrl;
        
        public string BaseUrl => _baseUrl.TrimEnd('/');
    }
}