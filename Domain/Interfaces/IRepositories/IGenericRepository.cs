using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(Guid id);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        Task DeleteAsync(Guid id);
        Task AddBulkAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> GetAll();
        void UpdateBulk(IEnumerable<TEntity> entities);
    }
}
