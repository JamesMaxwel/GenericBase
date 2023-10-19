using GenericBase.Domain.Interfaces;

namespace GenericBase.Infra.Data.Interfaces.Common.Generic
{
    public interface IGenericUpdateRepository<TEntity> where TEntity : class, IBase
    {
        void Update(TEntity entity);
    }
}
