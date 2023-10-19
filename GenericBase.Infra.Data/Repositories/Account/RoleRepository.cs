using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.DataContext;
using GenericBase.Infra.Data.Interfaces.Account;
using GenericBase.Infra.Data.Repositories.Common;

namespace GenericBase.Infra.Data.Repositories.Account
{
    internal class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(MyDbContext dbContext) : base(dbContext)
        {
        }
    }
}
