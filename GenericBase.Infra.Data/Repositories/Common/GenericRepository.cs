using GenericBase.Domain.Interfaces;
using GenericBase.Infra.Data.DataContext;
using GenericBase.Infra.Data.Interfaces.Common.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace GenericBase.Infra.Data.Repositories.Common
{
    internal class GenericRepository<TEntity> :
            IGenericCreateRepository<TEntity>,
            IGenericReadRepository<TEntity>,
            IGenericUpdateRepository<TEntity>,
            IGenericDeleteRepository<TEntity>

        where TEntity : class, IBase
    {
        protected readonly MyDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(MyDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        //Create
        public virtual async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) => await _dbSet.AddRangeAsync(entities);

        //Read
        public virtual async Task<TEntity?> GetFirstOrDefaultAsync(Guid id) => await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        public virtual async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }

            return await query.FirstOrDefaultAsync(predicate);
        }
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }

            return await query.ToListAsync();
        }
        public virtual async Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();

        //Update
        public virtual void Update(TEntity entity) => _dbSet.Update(entity);

        //Delete
        public virtual void Remove(TEntity entity) => _dbSet.Remove(entity);
        public virtual void RemoveRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);
    }
}
