using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.DataContext;
using GenericBase.Infra.Data.Interfaces.Account;
using GenericBase.Infra.Data.Repositories.Common;

namespace GenericBase.Infra.Data.Repositories.Account
{
    internal class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(MyDbContext dbContext) : base(dbContext)
        {
        }
    }
}
