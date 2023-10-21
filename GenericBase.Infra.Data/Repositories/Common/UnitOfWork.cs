using GenericBase.Infra.Data.DataContext;
using GenericBase.Infra.Data.Interfaces;
using GenericBase.Infra.Data.Interfaces.Account;
using GenericBase.Infra.Data.Interfaces.Common;
using GenericBase.Infra.Data.Repositories.Account;

namespace GenericBase.Infra.Data.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _context;

        private IUserRepository? _users;
        private IRoleRepository? _roles;
        private IPermissionRepository? _permissions;

        private ICustomerRepository? _customers;
        private IProductRepository? _products;
        private IOrderRepository? _orders;

        public UnitOfWork(MyDbContext context) => _context = context;

        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
        public IPermissionRepository Permissions => _permissions ??= new PermissionRepository(_context);

        public ICustomerRepository Customers => _customers ??= new CustomerRepository(_context);
        public IProductRepository Products => _products ??= new ProductRepository(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        private bool _disposed;
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    await _context.DisposeAsync();

                _disposed = true;
            }
        }
    }
}
