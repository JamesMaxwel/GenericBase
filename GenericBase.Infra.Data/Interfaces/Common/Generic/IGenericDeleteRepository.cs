using GenericBase.Domain.Interfaces;

namespace GenericBase.Infra.Data.Interfaces.Common.Generic
{
    public interface IGenericDeleteRepository<TEntity> where TEntity : class, IBase
    {
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
