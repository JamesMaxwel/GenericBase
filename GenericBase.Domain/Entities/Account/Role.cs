using GenericBase.Domain.Common;
using System.Collections.ObjectModel;

namespace GenericBase.Domain.Entities.Account
{
    public class Role : Base
    {
        public Role(string name, string slug, string? description)
        {
            Name = name;
            Slug = slug;
            Description = description;
        }

        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<User>? Users { get; set; } = new Collection<User>();
        public virtual ICollection<Permission> Permissions { get; set; } = new Collection<Permission>();

    }
}
