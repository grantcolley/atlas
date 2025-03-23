namespace Atlas.Core.Constants
{
    public static class Config
    {
        private const string ATLAS_API = "AtlasAPI";
        private const string CONNECTION_STRING = "AtlasConnection";
        private const string AUTH_DOMAIN = "Auth0:Domain";
        private const string AUTH_AUDIENCE = "Auth0:Audience";
        private const string AUTH_CLIENT_ID = "Auth0:ClientId";
        private const string AUTH_CLIENT_SECRET = "Auth0:ClientSecret";
        private const string CORS_POLICY = "CorsOrigins:Policy";
        private const string ORIGINS_URLS = "CorsOrigins:Urls";
        private const string DATABASE_CREATE = "Database:CreateDatabase";
        private const string DATABASE_SEED_DATA = "Database:GenerateSeedData";
        private const string DATABASE_SEED_LOGS = "Database:GenerateSeedLogs";

        // https://learn.microsoft.com/en-us/azure/app-service/configure-common?tabs=portal#configure-app-settings
        // In a default Linux app service or a custom Linux container, any nested JSON key structure in the app 
        // setting name needs to be configured differently for the key name.Replace any colon(:) with a double 
        // underscore(__), and replace any period(.) with a single underscore(_).
        // For example, ApplicationInsights:InstrumentationKey needs to be configured in App Service 
        // as ApplicationInsights__InstrumentationKey for the key name.

        private static readonly string _colon = ":";
        private static readonly string _doubleUnderscore = "__";

        public static string GET_ATLAS_API(bool useColon = true) { return useColon ? ATLAS_API : ATLAS_API.Replace(_colon, _doubleUnderscore); }
        public static string GET_CONNECTION_STRING(bool useColon = true) { return useColon ? CONNECTION_STRING : CONNECTION_STRING.Replace(_colon, _doubleUnderscore); }
        public static string GET_AUTH_DOMAIN(bool useColon = true) { return useColon ? AUTH_DOMAIN : AUTH_DOMAIN.Replace(_colon, _doubleUnderscore); }
        public static string GET_AUTH_AUDIENCE(bool useColon = true) { return useColon ? AUTH_AUDIENCE : AUTH_AUDIENCE.Replace(_colon, _doubleUnderscore); }
        public static string GET_AUTH_CLIENT_ID(bool useColon = true) { return useColon ? AUTH_CLIENT_ID : AUTH_CLIENT_ID.Replace(_colon, _doubleUnderscore); }
        public static string GET_AUTH_CLIENT_SECRET(bool useColon = true) { return useColon ? AUTH_CLIENT_SECRET : AUTH_CLIENT_SECRET.Replace(_colon, _doubleUnderscore); }
        public static string GET_CORS_POLICY(bool useColon = true) { return useColon ? CORS_POLICY : CORS_POLICY.Replace(_colon, _doubleUnderscore); }
        public static string GET_ORIGINS_URLS(bool useColon = true) { return useColon ? ORIGINS_URLS : ORIGINS_URLS.Replace(_colon, _doubleUnderscore); }
        public static string GET_DATABASE_CREATE(bool useColon = true) { return useColon ? DATABASE_CREATE : DATABASE_CREATE.Replace(_colon, _doubleUnderscore); }
        public static string GET_DATABASE_SEED_DATA(bool useColon = true) { return useColon ? DATABASE_SEED_DATA : DATABASE_SEED_DATA.Replace(_colon, _doubleUnderscore); }
        public static string GET_DATABASE_SEED_LOGS(bool useColon = true) { return useColon ? DATABASE_SEED_LOGS : DATABASE_SEED_LOGS.Replace(_colon, _doubleUnderscore); }
    }
}
