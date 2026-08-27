using Profiles.Models;

namespace Contracts.Profiles
{
    public interface IPlayerProfileSession
    {
        public void SetCurrentProfile(PlayerProfile playerProfile);
        public void ClearCurrentProfile();
    }
}