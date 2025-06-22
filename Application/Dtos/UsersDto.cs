namespace SupportSystemApp.Application.Dtos
{
    // Listing Users
    public class UsersDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAdmin { get; set; }
    }
}
