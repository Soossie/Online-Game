using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Contracts.Netcode;
using Netcode.Authentication;
using Profiles.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace Netcode
{
    public class NodeClient : IAuthenticationClient, IPlayerProfileClient, IRefreshAuthentication
    {
        private readonly string _baseUrl;
        
        public NodeClient(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException(nameof(baseUrl));
            
            _baseUrl = baseUrl;
        }

        public async Task<PlayerProfile> GetMyProfileAsync(AccessToken accessToken, CancellationToken ctx)
        {
            using UnityWebRequest request = UnityWebRequest.Get(_baseUrl + AppConstants.Api.ProfileEndpoint);
            request.SetRequestHeader("Authorization", "Bearer " + accessToken.Value);
            request.certificateHandler = new BypassCertificateHandler();
            await SendAsync(request, ctx);

            //TODO add redirect to login page
            if (request.responseCode == (long)HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Authentication rejected");
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.result);
                throw new InvalidOperationException($"Request failed ({request.responseCode}): {request.error}");
            }
            
            PlayerProfileResponseDto dto = 
                JsonUtility.FromJson<PlayerProfileResponseDto>(request.downloadHandler.text);
            return dto.FromDto();
        }
        
        public async Task<AuthenticationResult> LoginAsync(string email, string password, bool stayLoggedIn,
            CancellationToken ctx)
        {
            LoginRequestDto dto = new()
            {
                email = email,
                password = password,
                staySignedIn = stayLoggedIn
            };

            string json = JsonUtility.ToJson(dto);
            using UnityWebRequest request = new(
                _baseUrl + AppConstants.Api.LoginEndpoint, 
                UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.certificateHandler = new BypassCertificateHandler();
            request.SetRequestHeader("Content-Type", "application/json");
            await SendAsync(request, ctx);

            if (request.responseCode == (long) HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
            
            if (request.result != UnityWebRequest.Result.Success)
                throw new InvalidOperationException(
                    $"Request failed with code {request.responseCode}: {request.downloadHandler.text}");
            LoginResponseDto responseDto = JsonUtility.FromJson<LoginResponseDto>(request.downloadHandler.text);
            Debug.Log(responseDto.message);
            
            if (responseDto == null || string.IsNullOrWhiteSpace(responseDto.accessToken))
                throw new FormatException("Login response doesn't contain an access token.");
            return new AuthenticationResult(new AccessToken(responseDto.accessToken), new AccessToken(responseDto.refreshToken));
        }
        
        public async Task<AuthenticationResult> RefreshAsync(AccessToken refreshToken, CancellationToken ctx)
        {
            using UnityWebRequest request = UnityWebRequest.Get(_baseUrl + AppConstants.Api.RefreshEndpoint);
            request.SetRequestHeader("X-Refresh-Token", refreshToken.Value);
            request.certificateHandler = new BypassCertificateHandler();
            await SendAsync(request, ctx);

            //TODO add redirect to login page
            if (request.responseCode == (long)HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Authentication rejected");
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.result);
                throw new InvalidOperationException($"Request failed ({request.responseCode}): {request.error}");
            }
            
            RefreshResponseDto responseDto = JsonUtility.FromJson<RefreshResponseDto>(request.downloadHandler.text);
            return new AuthenticationResult(new AccessToken(responseDto.accessToken), refreshToken);
        }
        
        private static async Task SendAsync(UnityWebRequest request, CancellationToken ctx)
        {
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            
            
            while (!operation.isDone)
            {
                if (ctx.IsCancellationRequested)
                {
                    Debug.LogException(new OperationCanceledException());
                    request.Abort();
                    ctx.ThrowIfCancellationRequested();
                }

                await Task.Yield();
            }
            
            ctx.ThrowIfCancellationRequested();
        }
    }
}