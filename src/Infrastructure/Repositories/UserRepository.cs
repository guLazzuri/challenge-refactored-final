using challenge.Domain.Entities;
using challenge.Domain.Interfaces;

using challenge.Domain.Enums;
using challenge.Domain.ValueObjects;
using System.Linq.Expressions;
using MongoDB.Driver;


namespace challenge.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<User> _repository;

        // O construtor base é dummy, pois a injeção será feita via IMongoRepository
        public UserRepository(IMongoRepository<User> repository) 
        {
            _repository = repository;
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // O Value Object Email não é mapeado diretamente pelo MongoDB.Driver. 
            // A busca deve ser feita pelo campo Address dentro do objeto Email.
            return await _repository.FindAsync(u => u.Email.Value == email.ToLowerInvariant()).ContinueWith(t => t.Result.FirstOrDefault());
        }

        public async Task<User?> GetByDocumentAsync(string document)
        {
            return await _repository.FindAsync(u => u.Document == document).ContinueWith(t => t.Result.FirstOrDefault());
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<User>> GetByTypeAsync(UserType type)
        {
            return await _repository.FindAsync(u => u.Type == type);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _repository.FindAsync(u => u.IsActive);
        }

        public async Task AddAsync(User user)
        {
            await _repository.AddAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _repository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user != null;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _repository.FindAsync(u => u.Email.Value == email.ToLowerInvariant()).ContinueWith(t => t.Result.FirstOrDefault());
            return user != null;
        }

        public async Task<bool> DocumentExistsAsync(string document)
        {
            var user = await _repository.FindAsync(u => u.Document == document).ContinueWith(t => t.Result.FirstOrDefault());
            return user != null;
        }
    }
}
