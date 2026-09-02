using UnityEngine;

namespace Netcode
{
    [CreateAssetMenu(fileName = "NetworkSettingsSO", menuName = "Verkkopeli/NetworkSettingsSO")]
    public class NetworkSettingsSO: ScriptableObject
    {
        [SerializeField] private string baseUrl;
        
        public string BaseUrl => baseUrl.TrimEnd('/');
    }
}