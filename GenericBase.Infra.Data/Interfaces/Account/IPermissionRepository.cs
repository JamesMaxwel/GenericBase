using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.Interfaces.Common.Generic;

namespace GenericBase.Infra.Data.Interfaces.Account
{
    public interface IPermissionRepository :
        IGenericCreateRepository<Permission>,
        IGenericReadRepository<Permission>,
        IGenericUpdateRepository<Permission>,
        IGenericDeleteRepository<Permission>
    {
    }
}