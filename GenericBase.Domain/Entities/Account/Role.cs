using GenericBase.Domain.Common;

namespace GenericBase.Domain.Entities.Account
{
    public class Role : Base
    {
        public Role(string name, string slug)
        {
            Name = name;
            Slug = slug;
        }

        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    }
}
