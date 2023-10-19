using GenericBase.Domain.Entities;
using GenericBase.Infra.Data.Interfaces.Common.Generic;

namespace GenericBase.Infra.Data.Interfaces
{
    public interface ICustomerRepository :
        IGenericCreateRepository<Customer>,
        IGenericReadRepository<Customer>,
        IGenericUpdateRepository<Customer>,
        IGenericDeleteRepository<Customer>
    {
    }
}
