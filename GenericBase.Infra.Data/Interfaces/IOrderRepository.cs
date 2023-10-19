using GenericBase.Domain.Entities;
using GenericBase.Infra.Data.Interfaces.Common.Generic;

namespace GenericBase.Infra.Data.Interfaces
{
    public interface IOrderRepository :
        IGenericCreateRepository<Order>,
        IGenericReadRepository<Order>,
        IGenericUpdateRepository<Order>,
        IGenericDeleteRepository<Order>
    {
    }
}