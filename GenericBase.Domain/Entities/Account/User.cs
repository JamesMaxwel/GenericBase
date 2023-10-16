using GenericBase.Domain.Common;
using GenericBase.Domain.Common.Security;
using GenericBase.Domain.Enums;
using System.Collections.ObjectModel;

namespace GenericBase.Domain.Entities.Account
{
    public class User : Base
    {
        private User() { }
        public User(string email, string firstName, string lastName, string password) : this()
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Type = UserTypeEmum.Guest;

            (PasswordHash, Salt) = PassowrdHasher.Hash(password);
        }

        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();

        public string? Description { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new Collection<Role>();
        public virtual ICollection<Permission> Permissions { get; set; } = new Collection<Permission>();

        public UserTypeEmum Type { get; set; }

        public string Salt { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTimeOffset LockoutEnd { get; private set; } = DateTimeOffset.MinValue;
        public bool IsLockedOut => LockoutEnd >= DateTimeOffset.UtcNow;
        public bool IsEnabled { get; set; }

        public bool IsSamePassword(string password) => PassowrdHasher.Verify(password, Salt, PasswordHash);
        public void SetPassword(string password) => (PasswordHash, Salt) = PassowrdHasher.Hash(password);
        public void SetLockAt(DateTimeOffset lockoutEnd) => LockoutEnd = lockoutEnd;
        public void SetUnLock() => LockoutEnd = DateTimeOffset.MinValue;

    }
}
