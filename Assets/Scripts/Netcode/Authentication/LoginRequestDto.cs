using System;

namespace Netcode.Authentication
{
    [Serializable]
    public sealed class LoginRequestDto
    {
        public string email;
        public string password;
        public bool staySignedIn;
    }
}