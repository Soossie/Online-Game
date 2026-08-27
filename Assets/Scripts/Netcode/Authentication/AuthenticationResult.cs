namespace Netcode.Authentication
{
    public struct AuthenticationResult
    {
        public AccessToken AccessToken { get; }
        public AccessToken RefreshToken { get; }
        
        public AuthenticationResult(AccessToken accessToken, AccessToken refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}