using SupportSystem.Application.Interfaces;
using SupportSystem.Domain.Entities;
using BCrypt.Net;

namespace SupportSystem.Application.Services
{
    public class AuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Giriş işlemi
        public async Task<User?> AuthenticateAsync(string usernameOrEmail, string password)
        {
            var user = await _userRepository.GetByUsernameOrEmailAsync(usernameOrEmail);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid)
            {
                return null;
            }

            return user;
        }

        // Kayıt işlemi
        public async Task<User?> RegisterAsync(string username, string email, string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            return user;
        }
    }

}
