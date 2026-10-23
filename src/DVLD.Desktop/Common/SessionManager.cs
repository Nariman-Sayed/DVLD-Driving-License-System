namespace DVLD.Desktop.Common
{
    public static class SessionManager
    {
        public static string? Token { get; set; }
        public static int UserID { get; set; }
        public static string Username { get; set; } = string.Empty;

        public static bool IsLoggedIn => !string.IsNullOrEmpty(Token);

        public static void ClearSession()
        {
            Token = null;
            UserID = 0;
            Username = string.Empty;
        }
    }
}