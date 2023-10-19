using GenericBase.Domain.Entities;
using GenericBase.Infra.Data.DataContext;
using GenericBase.Infra.Data.Interfaces;
using GenericBase.Infra.Data.Repositories.Common;

namespace GenericBase.Infra.Data.Repositories
{
    internal class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(MyDbContext dbContext) : base(dbContext)
        {
        }
    }
}
