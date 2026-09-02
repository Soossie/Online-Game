using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Netcode;
using Contracts.Profiles;
using Profiles.Models;
using UnityEngine;

namespace Netcode.Authentication
{
    public sealed class AuthenticationService: IAuthenticationService // Includes IAuthenticationContext
    {
        public bool IsAuthenticated => _currentAuth.HasValue;
        private AuthenticationResult? _currentAuth;
        private readonly IAuthenticationClient _authClient;
        private readonly IRefreshAuthentication _refreshAuth;
        private readonly IPlayerProfileClient _profileClient;
        private readonly IAuthenticationSessionStore _authSessionStore;
        private readonly IPlayerProfileSession _profileSession;

        public AuthenticationService(
            IAuthenticationClient authClient, 
            IRefreshAuthentication refreshAuth,
            IPlayerProfileClient profileClient,
            IAuthenticationSessionStore authSessionStore,
            IPlayerProfileSession profileSession)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
            _refreshAuth = refreshAuth ?? throw new ArgumentNullException(nameof(refreshAuth));
            _profileClient = profileClient ?? throw new ArgumentNullException(nameof(profileClient));
            _authSessionStore = authSessionStore ?? throw new ArgumentNullException(nameof(authSessionStore));
            _profileSession = profileSession ?? throw new ArgumentNullException(nameof(profileSession));
        }
        
        public async Task LoginAsync(string email, string password, bool stayLoggedIn, CancellationToken ctx)
        {
            AuthenticationResult auth = await _authClient.LoginAsync(email, password, stayLoggedIn, ctx);
            PlayerProfile profile = await _profileClient.GetMyProfileAsync(auth.AccessToken, ctx);
            await _authSessionStore.SaveAsync(auth, ctx);
            ctx.ThrowIfCancellationRequested();
            _currentAuth = auth;
            _profileSession.SetCurrentProfile(profile);
        }

        public async Task LogoutAsync(CancellationToken ctx)
        {
            await _authSessionStore.ClearAsync(ctx);
            _currentAuth = null;
            _profileSession.ClearCurrentProfile();
        }
        
        public async Task<bool> TryRestoreSessionASync(CancellationToken ctx)
        {
            Debug.Log("Got here");
            AuthenticationResult? storedAuth = await _authSessionStore.LoadASync(ctx);
            Debug.Log("Got past load");
            if (!storedAuth.HasValue)
            {
                Debug.LogWarning("No authentication session found");
                return false;
            }

            try
            {
                Debug.Log("Trying to restore session");
                PlayerProfile profile = await _profileClient.GetMyProfileAsync(new AccessToken(), ctx); //TODO
                ctx.ThrowIfCancellationRequested();
                _currentAuth = storedAuth.Value;
                _profileSession.SetCurrentProfile(profile);
                Debug.Log("Access token valid");
                return true;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                Debug.Log("Access token expired, refreshing...");
                if (!string.IsNullOrEmpty(storedAuth.Value.RefreshToken.Value))
                {
                    try
                    {
                        AuthenticationResult auth = await _refreshAuth.RefreshAsync(storedAuth.Value.RefreshToken, ctx);
                        await _authSessionStore.SaveAsync(auth, ctx);
                        _currentAuth = auth;
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                        Debug.LogError("Failed to refresh access token, login again");
                        await _authSessionStore.ClearAsync(ctx);
                        _currentAuth = null;
                        _profileSession.ClearCurrentProfile();
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("Failed to refresh access token, login again");
                    await _authSessionStore.ClearAsync(ctx);
                    _currentAuth = null;
                    _profileSession.ClearCurrentProfile();
                    return false;
                }
                
                ctx.ThrowIfCancellationRequested();
                
                if (!string.IsNullOrEmpty(_currentAuth.Value.AccessToken.Value))
                {
                    // Got a new access token, try logging again
                    Debug.Log("New access token acquired, logging in...");
                    try
                    {
                        PlayerProfile profile = await _profileClient.GetMyProfileAsync(_currentAuth.Value.AccessToken, ctx);
                        ctx.ThrowIfCancellationRequested();
                        _currentAuth = storedAuth.Value;
                        _profileSession.SetCurrentProfile(profile);
                        return true;
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        Debug.Log("Failed to refresh access token, login again");
                        await _authSessionStore.ClearAsync(ctx);
                        _currentAuth = null;
                        _profileSession.ClearCurrentProfile();
                        Debug.LogException(e);
                        return false;
                    }
                }
                Debug.LogError("Failed to refresh access token, login again");
                await _authSessionStore.ClearAsync(ctx);
                _currentAuth = null;
                _profileSession.ClearCurrentProfile();
                return false;
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return false;
            }
        }
        
        public bool TryGetAccessToken(out AccessToken accessToken)
        {
            if (_currentAuth.HasValue)
            {
                accessToken = _currentAuth.Value.AccessToken;
                return true;
            }

            accessToken = default;
            return false;
        }

        public bool TryGetRefreshToken(out AccessToken refreshToken)
        {
            if (_currentAuth.HasValue)
            {
                refreshToken = _currentAuth.Value.RefreshToken;
                return true;
            }

            refreshToken = default;
            return false;
        }
    }
}