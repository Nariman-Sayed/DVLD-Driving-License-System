namespace DVLD.Shared
{
    public class UserDto
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}