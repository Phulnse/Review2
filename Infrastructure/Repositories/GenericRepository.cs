using Domain.Entities;
using Domain.Interfaces.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly SRMSContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(SRMSContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstAsync(x => x.Id == id);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task AddBulkAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public void UpdateBulk(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }
    }
}
