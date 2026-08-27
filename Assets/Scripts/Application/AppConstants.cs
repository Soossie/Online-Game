namespace Application
{
    public static class AppConstants
    {
        public static class Api
        {
            public const string LoginEndpoint = "/api/auth/login";
            public const string ProfileEndpoint = "/api/profiles/me";
            public const string RefreshEndpoint = "/api/auth/refresh";
        }
        
        public static class Authentication
        {
            public const string DataPath = "auth_data.json";
        }
    }
}