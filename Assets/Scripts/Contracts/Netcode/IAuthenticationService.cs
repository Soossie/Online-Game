using System.Threading;
using System.Threading.Tasks;

namespace Contracts.Netcode
{
    public interface IAuthenticationService: IAuthenticationContext
    {
        public Task LoginAsync(string email, string password, bool stayLoggedIn, CancellationToken ctx);
        public Task LogoutAsync(CancellationToken ctx);
        public Task<bool> TryRestoreSessionASync(CancellationToken ctx);
    }
}