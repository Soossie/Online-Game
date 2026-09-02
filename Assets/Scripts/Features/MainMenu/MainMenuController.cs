using System;
using Application;
using Contracts.Profiles;
using Contracts.SceneControl;
using Profiles;
using Profiles.Models;
using SceneManagement;
using TMPro;
using UnityEngine;

namespace Features.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI greetText;
        private PlayerProfile _playerProfile;
        private ISceneFlowController _sceneFlowController;
        private IPlayerProfileContext _profileContext;
        
        public void Bind(ISceneFlowController sceneFlowController, IPlayerProfileContext playerProfileContext)
        {
            
            _sceneFlowController = sceneFlowController 
                                   ?? throw new ArgumentNullException(nameof(sceneFlowController));
            _profileContext = playerProfileContext 
                                    ?? throw new ArgumentNullException(nameof(playerProfileContext));
        }

        public void Greet()
        {
            var hasProfile = _profileContext.TryGetCurrentProfile(out _playerProfile);
            if (!hasProfile)
                throw new ArgumentNullException(nameof(_playerProfile));
            
            var hexColor = $"#{_playerProfile.PlayerColor.Red:X2}" +
                           $"{_playerProfile.PlayerColor.Green:X2}" +
                           $"{_playerProfile.PlayerColor.Blue:X2}";
            
            greetText.text = $"Welcome <color={hexColor}>{_playerProfile.DisplayName}</color>";
        }
    }
}
