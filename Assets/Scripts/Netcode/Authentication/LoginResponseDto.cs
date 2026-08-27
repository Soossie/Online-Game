using System;

namespace Netcode.Authentication
{
    [Serializable]
    public sealed class LoginResponseDto
    {
        public string accessToken;
        public string refreshToken;
        public string message;
    }
}