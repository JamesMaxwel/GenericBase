using GenericBase.Domain.Common;
using GenericBase.Domain.Common.Security;

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

            (PasswordHash, Salt) = PassowrdHasher.Hash(password);
        }

        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserType Type { get; set; } = UserType.Guest;
        public string? Description { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        public string Salt { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;

        public DateTimeOffset? LockoutEnd { get; private set; }

        public string? LastJti { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsLockedOut => LockoutEnd >= DateTimeOffset.UtcNow;
        public bool IsSamePassword(string password) => PassowrdHasher.Verify(password, Salt, PasswordHash);
        public void SetPassword(string password) => (PasswordHash, Salt) = PassowrdHasher.Hash(password);
        public void SetLockAt(DateTimeOffset lockoutEnd) => LockoutEnd = lockoutEnd;
        public void SetUnLock() => LockoutEnd = DateTimeOffset.MinValue;

    }
    public enum UserType
    {
        SuperAdmin = 5,
        Admin = 4,
        Auditor = 3,
        Employeer = 2,
        Customer = 1,
        Guest = 0
    }
}
