using GenericBase.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace GenericBase.Infra.Data.Interfaces.Common.Generic
{
    public interface IGenericReadRepository<TEntity> where TEntity : class, IBase
    {
        Task<TEntity?> GetFirstOrDefaultAsync(Guid id);
        Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
            params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAllAsync(params Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
