using GenericBase.Domain.Entities.Account;

namespace GenericBase.Infra.Data.DataContext.EntityRelations
{
    internal class UserVsRole
    {
        public virtual User? User { get; set; }
        public Guid UserId { get; set; }
        public virtual Role? Role { get; set; }
        public Guid RoleId { get; set; }
    }
}
