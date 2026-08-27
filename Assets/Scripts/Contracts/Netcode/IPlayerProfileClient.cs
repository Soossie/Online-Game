using System.Threading;
using System.Threading.Tasks;
using Netcode.Authentication;
using Profiles.Models;

namespace Contracts.Netcode
{
    public interface IPlayerProfileClient
    {
        public Task<PlayerProfile> GetMyProfileAsync(AccessToken accessToken, CancellationToken ctx);
    }
}