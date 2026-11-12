namespace WaterRefillingStation.Security
{
    // Holds the logged-in user. Use Session.UserID for CreatedBy / ReceivedBy columns.
    public static class Session
    {
        public static int UserID { get; private set; }
        public static string Username { get; private set; }
        public static string FullName { get; private set; }
        public static string Role { get; private set; }

        public static bool IsLoggedIn => UserID > 0;
        public static bool IsAdmin => Role == "Admin";

        public static void Start(int userId, string username, string fullName, string role)
        {
            UserID = userId;
            Username = username;
            FullName = fullName;
            Role = role;
        }

        public static void End()
        {
            UserID = 0;
            Username = null;
            FullName = null;
            Role = null;
        }
    }
}