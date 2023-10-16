using GenericBase.Domain.Common;
using System.Collections.ObjectModel;

namespace GenericBase.Domain.Entities.Account
{
    public class Permission : Base
    {
        public Permission(string key, string value, bool isPublicInToken)
        {
            Key = key;
            Value = value;
            IsPublicInToken = isPublicInToken;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsPublicInToken { get; set; }

        public virtual ICollection<User> Users { get; set; } = new Collection<User>();
        public virtual ICollection<Role> Roles { get; set; } = new Collection<Role>();

    }
}
