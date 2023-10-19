using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.DataContext;
using GenericBase.Infra.Data.Interfaces.Account;
using GenericBase.Infra.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GenericBase.Infra.Data.Repositories.Account
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MyDbContext dbContext) : base(dbContext) { }

        public async Task<User?> GetFirstOrDefaultWithCredentialsAsync(Guid id)
        {
            return await _dbSet
                .Include(o => o.Roles)
                    .ThenInclude(o => o.Permissions)
                .Include(o => o.Permissions)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
