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
        public static string SessionToken = "JWTToken";
        public static string CurrentVersion = "v2";
        public static string Admin = "Admin";
        public static string Customer = "Customer";
        public enum ContentType
        {
            Json,
            MultiPartFormData
        }
    }
}
