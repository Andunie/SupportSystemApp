using SupportSystem.Domain.Entities;
using SupportSystemApp.Application.Dtos;

namespace SupportSystem.Application.Interfaces
{
    public interface IUserRepository
    {
		Task<User?> GetByIdAsync(Guid id);
		Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
        Task<List<UsersDto>> GetPaginatedUsersAsync(int pageNumber, int pageSize);
        Task<int> GetTotalUserCountAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task SaveChangesAsync();
    }
}
