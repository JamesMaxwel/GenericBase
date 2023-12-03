using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.DataContext;
using GenericBase.Infra.Data.Interfaces.Account;
using GenericBase.Infra.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericBase.Infra.Data.Repositories.Account
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MyDbContext dbContext) : base(dbContext) { }

        public async Task<User?> GetFirstOrDefaultWithCredentialsAsync(Guid id)
            => await GetFirstOrDefaultWithCredentialsAsync(x => x.Id == id);


        public async Task<User?> GetFirstOrDefaultWithCredentialsAsync(Expression<Func<User, bool>> expression)
        {
            return await _dbSet
               .Include(o => o.Roles)
                   .ThenInclude(o => o.Permissions)
               .Include(o => o.Permissions)
               .FirstOrDefaultAsync(expression);
        }
    }
}
