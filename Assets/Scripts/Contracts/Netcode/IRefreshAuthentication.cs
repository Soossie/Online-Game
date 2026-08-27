using System.Threading;
using System.Threading.Tasks;
using Netcode.Authentication;

namespace Contracts.Netcode
{
    public interface IRefreshAuthentication
    {
        public Task<AuthenticationResult> RefreshAsync(AccessToken refreshToken,
            CancellationToken ctx);
    }
}