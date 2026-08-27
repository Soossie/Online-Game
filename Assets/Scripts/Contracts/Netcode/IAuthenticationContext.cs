using Netcode.Authentication;

namespace Contracts.Netcode
{
    public interface IAuthenticationContext
    {
        public bool IsAuthenticated { get; }
        public bool TryGetAccessToken(out AccessToken accessToken);
    }
}