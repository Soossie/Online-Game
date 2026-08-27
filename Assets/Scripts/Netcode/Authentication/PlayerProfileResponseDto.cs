using System;

namespace Netcode.Authentication
{
    [Serializable]
    public class PlayerProfileResponseDto
    {
        public string playerId;
        public string displayName;
        public string playerColor;
    }
}