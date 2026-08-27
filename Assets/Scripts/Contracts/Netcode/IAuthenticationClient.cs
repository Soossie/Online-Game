using System.Threading;
using System.Threading.Tasks;
using Netcode.Authentication;

namespace Contracts.Netcode
{
    public interface IAuthenticationClient
    {
        public Task<AuthenticationResult> LoginAsync(
            string email,
            string password,
            bool stayLoggedIn,
            CancellationToken ctx
            );
    }
}