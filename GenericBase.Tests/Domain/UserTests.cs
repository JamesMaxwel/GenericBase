using GenericBase.Domain.Entities.Account;

namespace GenericBase.Tests.Domain
{
    public class UserTests
    {
        [Fact]
        public void SetPassword_UpdatesPasswordHashAndSalt()
        {
            // Arrange
            var user = new User("test@example.com", "John", "Doe", "password123");

            // Act
            user.SetPassword("newpassword456");

            // Assert
            Assert.NotNull(user.PasswordHash);
            Assert.NotNull(user.Salt);
            Assert.NotEqual("password123", user.PasswordHash);
        }

        [Fact]
        public void IsSamePassword_ReturnsTrueForCorrectPassword()
        {
            // Arrange
            var user = new User("test@example.com", "John", "Doe", "password123");

            // Act
            var isSamePassword = user.IsSamePassword("password123");

            // Assert
            Assert.True(isSamePassword);
        }

        [Fact]
        public void IsSamePassword_ReturnsFalseForIncorrectPassword()
        {
            // Arrange
            var user = new User("test@example.com", "John", "Doe", "password123");

            // Act
            var isSamePassword = user.IsSamePassword("incorrectpassword");

            // Assert
            Assert.False(isSamePassword);
        }

        [Fact]
        public void SetLockAt_SetsLockoutEndCorrectly()
        {
            // Arrange
            var user = new User("test@example.com", "John", "Doe", "password123");
            var lockoutEnd = DateTimeOffset.UtcNow.AddMinutes(30);

            // Act
            user.SetLockAt(lockoutEnd);

            // Assert
            Assert.NotNull(user.LockoutEnd);
            Assert.Equal(lockoutEnd, user.LockoutEnd);
        }

        [Fact]
        public void SetUnLock_ClearsLockoutEnd()
        {
            // Arrange
            var user = new User("test@example.com", "John", "Doe", "password123");
            user.SetLockAt(DateTimeOffset.UtcNow.AddMinutes(30));

            // Act
            user.SetUnLock();

            // Assert
            Assert.Null(user.LockoutEnd);
        }
    }
}