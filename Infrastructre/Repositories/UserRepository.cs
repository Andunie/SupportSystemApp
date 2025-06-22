using Microsoft.EntityFrameworkCore;
using SupportSystem.Application.Interfaces;
using SupportSystem.Domain.Entities;
using SupportSystem.Infrastructure.Data;
using SupportSystemApp.Application.Dtos;

namespace SupportSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UsersDto>> GetPaginatedUsersAsync(int pageNumber, int pageSize)
        {
            return await _context.Users
            .OrderBy(u => u.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UsersDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                CreatedAt = u.CreatedAt,
                IsAdmin = u.IsAdmin
            })
            .ToListAsync();
        }

        public async Task<int> GetTotalUserCountAsync()
        {
            return await _context.Users.CountAsync();
        }
    }
}
