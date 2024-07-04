namespace MagicVilla_Utility
{
    public static class SD
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        public static string AccessToken = "JWTToken";
        public static string CurrentVersion = "v2";
        public static string Admin = "admin";
        public static string Customer = "customer";
        public enum ContentType
        {
            Json,
            MultiPartFormData
        }
    }
}
