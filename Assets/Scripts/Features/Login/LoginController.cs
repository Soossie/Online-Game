using System;
using System.Threading.Tasks;
using Contracts.Netcode;
using Contracts.SceneControl;
using SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Login
{
    public sealed class LoginController : MonoBehaviour
    {
        [SerializeField] private ScenePathSO nextSceneSo;
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Toggle stayLoggedInToggle;
        
        private ISceneFlowController _sceneFlowController;
        private IAuthenticationService _authenticationService;
        
        public void Bind(ISceneFlowController sceneFlowController, IAuthenticationService authService)
        {
            
            _sceneFlowController = sceneFlowController 
                                   ?? throw new ArgumentNullException(nameof(sceneFlowController));
            _authenticationService = authService 
                                     ?? throw new ArgumentNullException(nameof(authService));
        }

        public async void Login()
        {
            try
            {
                statusText.color = Color.papayaWhip;
                statusText.text = "Logging in...";
                await _authenticationService.LoginAsync(
                    emailInputField.text,
                    passwordInputField.text, stayLoggedInToggle.isOn,
                    destroyCancellationToken);
                
                destroyCancellationToken.ThrowIfCancellationRequested();
                statusText.color = Color.green;
                statusText.text = "Logged in";
                await Task.Delay(1000);
                _sceneFlowController.ChangePrimaryScene(nextSceneSo);
            }
            catch (Exception exception)
            {
                statusText.color = Color.red;
                statusText.text = "Login failed";
                Debug.LogException(exception);
            }
        }
    }
}