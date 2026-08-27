using System;

namespace Netcode.Authentication
{
    [Serializable]
    public sealed class AuthenticationSessionDto
    {
        public string accessToken;
        public string refreshToken;
    }
}