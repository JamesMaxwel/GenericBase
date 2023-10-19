using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.Interfaces.Common.Generic;

namespace GenericBase.Infra.Data.Interfaces.Account
{
    public interface IRoleRepository :
        IGenericCreateRepository<Role>,
        IGenericReadRepository<Role>,
        IGenericUpdateRepository<Role>,
        IGenericDeleteRepository<Role>
    {
    }
}