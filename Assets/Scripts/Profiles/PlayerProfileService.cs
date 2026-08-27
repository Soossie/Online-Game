using System;
using Contracts.Profiles;
using Profiles.Models;

namespace Profiles
{
    public class PlayerProfileService: IPlayerProfileSession, IPlayerProfileContext
    {
        public bool HasProfile => _currentProfile.HasValue;
        public PlayerProfile CurrentProfile => _currentProfile 
                                               ?? throw new InvalidOperationException(nameof(_currentProfile));
        private PlayerProfile? _currentProfile;
        public void SetCurrentProfile(PlayerProfile playerProfile)
        {
            _currentProfile = playerProfile;
        }

        public void ClearCurrentProfile()
        {
            _currentProfile = null;
        }
        public bool TryGetCurrentProfile(out PlayerProfile playerProfile)
        {
            // TODO Olisiko voinut tehdä return _currentProfile.value ?? throw new etc..
            if (_currentProfile.HasValue)
            {
                playerProfile = _currentProfile.Value;
                return true;
            }

            playerProfile = default;
            return false;
        }
    }
}