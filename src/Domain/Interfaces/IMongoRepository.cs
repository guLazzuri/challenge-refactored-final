using System.Linq.Expressions;
using challenge.Domain.Entities;

namespace challenge.Domain.Interfaces
{
    public interface IMongoRepository<T> where T : MongoEntity
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> filterExpression);
    }
}
