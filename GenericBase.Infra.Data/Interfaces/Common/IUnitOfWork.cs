using GenericBase.Infra.Data.Interfaces.Account;

namespace GenericBase.Infra.Data.Interfaces.Common
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }

        ICustomerRepository Customers { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        Task<int> SaveChangesAsync();

    }
}
