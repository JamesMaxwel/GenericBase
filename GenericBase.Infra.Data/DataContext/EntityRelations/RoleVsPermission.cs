using GenericBase.Domain.Entities.Account;

namespace GenericBase.Infra.Data.DataContext.EntityRelations
{
    public class RoleVsPermission
    {
        public virtual Role? Role { get; set; }
        public Guid RoleId { get; set; }
        public virtual Permission? Permission { get; set; }
        public Guid PermissionId { get; set; }
    }
}
