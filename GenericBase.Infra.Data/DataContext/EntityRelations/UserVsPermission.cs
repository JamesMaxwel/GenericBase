using GenericBase.Domain.Entities.Account;

namespace GenericBase.Infra.Data.DataContext.EntityRelations
{
    public class UserVsPermission
    {
        public virtual User? User { get; set; }
        public Guid UserId { get; set; }
        public virtual Permission? Permission { get; set; }
        public Guid PermissionId { get; set; }
    }
}
