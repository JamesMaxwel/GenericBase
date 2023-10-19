using GenericBase.Domain.Entities;
using GenericBase.Infra.Data.Interfaces.Common.Generic;

namespace GenericBase.Infra.Data.Interfaces
{
    public interface IProductRepository :
        IGenericCreateRepository<Product>,
        IGenericReadRepository<Product>,
        IGenericUpdateRepository<Product>,
        IGenericDeleteRepository<Product>
    {
    }
}