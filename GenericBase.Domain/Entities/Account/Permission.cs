using GenericBase.Domain.Common;

namespace GenericBase.Domain.Entities.Account
{
    public class Permission : Base
    {
        public Permission(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    }
}
