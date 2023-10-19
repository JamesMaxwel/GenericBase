using GenericBase.Domain.Interfaces;

namespace GenericBase.Infra.Data.Interfaces.Common.Generic
{
    public interface IGenericCreateRepository<TEntity> where TEntity : class, IBase
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
    }
}
