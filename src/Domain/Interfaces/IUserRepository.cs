using challenge.Domain.Entities;
using challenge.Domain.Enums;

namespace challenge.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByDocumentAsync(string document);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetByTypeAsync(UserType type);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> DocumentExistsAsync(string document);
    }
}

