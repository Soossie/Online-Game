using System.Threading;
using System.Threading.Tasks;
using Netcode.Authentication;

namespace Contracts.Netcode
{
    public interface IAuthenticationSessionStore
    {
        public Task<AuthenticationResult?> LoadASync(CancellationToken ctx);
        public Task SaveAsync(AuthenticationResult authResult, CancellationToken ctx);
        public Task ClearAsync(CancellationToken ctx);
    }
}