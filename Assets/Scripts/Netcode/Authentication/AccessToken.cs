using System;

namespace Netcode.Authentication
{
    public struct AccessToken
    {
        public string Value { get; }

        public AccessToken(string tokenString)
        {
            /*
            if (string.IsNullOrWhiteSpace(tokenString))
                throw new ArgumentException(nameof(tokenString));
                */

            Value = tokenString;
        }

        public override string ToString()
        {
            return "[Token]";
        }
    }
}