using GenericBase.Application.Constants;
using GenericBase.Application.Helpers;
using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.DataContext;
using Microsoft.Extensions.Logging;
using System.Data;

namespace GenericBase.Infra.Ioc
{
    public class DbInitializer
    {
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(ILogger<DbInitializer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public static void Initialize(MyDbContext dbContext, ILogger<DbInitializer> logger)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
                dbContext.Database.EnsureCreated();

                SeedPermissions(dbContext, logger);
                SeedRoles(dbContext, logger);
                SeedUsers(dbContext, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during database initialization.");
            }
        }
        private static void SeedPermissions(MyDbContext dbContext, ILogger logger)
        {
            if (dbContext.Permissions.Any())
                return;

            var claims = new HashSet<Permission>();
            var modules = typeof(ModuleTypes).GetFields().Where(x => x.IsLiteral && !x.IsInitOnly);

            foreach (var module in modules)
            {
                claims.Add(new Permission(module.Name, OperationTypes.Create));
                claims.Add(new Permission(module.Name, OperationTypes.Read));
                claims.Add(new Permission(module.Name, OperationTypes.Update));
                claims.Add(new Permission(module.Name, OperationTypes.Delete));

                claims.Add(new Permission(module.Name, OperationTypes.UpdateMyContent));
                claims.Add(new Permission(module.Name, OperationTypes.UpdateTeamContent));

                claims.Add(new Permission(module.Name, OperationTypes.DeleteMyContent));
                claims.Add(new Permission(module.Name, OperationTypes.DeleteTeamContent));
            }

            dbContext.Permissions.AddRange(claims);

            if (dbContext.SaveChanges() > 0)
                logger.LogInformation("SeedPermissions completed successfully.");
        }
        private static void SeedRoles(MyDbContext dbContext, ILogger logger)
        {
            if (dbContext.Roles.Any())
                return;

            var roles = new HashSet<Role>();

            var modules = typeof(ModuleTypes).GetFields().Where(x => x.IsLiteral && !x.IsInitOnly);
            var roleTypes = typeof(RoleTypes).GetFields().Where(x => x.IsLiteral && !x.IsInitOnly);

            foreach (var module in modules)
            {
                foreach (var roleType in roleTypes)
                {
                    var role = new Role($"{roleType.Name} {module.Name}", $"{roleType.Name}.{module.Name}");
                    var operations = new List<string>();

                    switch (roleType.Name)
                    {
                        case RoleTypes.Manager:
                            operations.Add(OperationTypes.Create);
                            operations.Add(OperationTypes.Read);
                            operations.Add(OperationTypes.Update);
                            operations.Add(OperationTypes.Delete);
                            break;

                        case RoleTypes.Supervisor:
                            operations.Add(OperationTypes.Create);
                            operations.Add(OperationTypes.Read);
                            operations.Add(OperationTypes.UpdateTeamContent);
                            operations.Add(OperationTypes.DeleteTeamContent);
                            break;

                        case RoleTypes.Operator:
                            operations.Add(OperationTypes.Create);
                            operations.Add(OperationTypes.Read);
                            operations.Add(OperationTypes.UpdateMyContent);
                            operations.Add(OperationTypes.DeleteMyContent);
                            break;

                        default:
                            break;
                    }

                    role.Permissions = dbContext.Permissions
                      .Where(x => x.Key == module.Name && operations.Contains(x.Value))
                      .ToList();

                    roles.Add(role);
                }
            }
            dbContext.Roles.AddRange(roles);

            if (dbContext.SaveChanges() > 0)
                logger.LogInformation("SeedRoles completed successfully.");
        }
        private static void SeedUsers(MyDbContext dbContext, ILogger logger)
        {
            if (dbContext.Users.Any())
                return;

            var names = new HashSet<string>() { "Ana", "Carlos", "Fernanda", "Ricardo",
                "Gabriela", "Lucas", "Isabela", "Matheus", "Juliana", "Pedro", "Camila",
                "Anderson", "Mariana", "Renato", "Beatriz", "João", "Amanda", "Diego",
                "Tatiane", "Vinícius" };
            var surnames = new HashSet<string>() { "Silva", "Santos", "Oliveira", "Souza",
                "Pereira", "Lima", "Costa", "Ferreira", "Rodrigues", "Almeida", "Nascimento",
                "Cavalcante", "Gomes", "Martins", "Araújo", "Carvalho", "Sousa", "Ribeiro",
                "Lopes", "Melo" };

            var users = new List<User>();
            var roles = dbContext.Roles.ToList();
            var permissions = dbContext.Permissions.ToList();

            var random = new Random();

            while (names.Any() && surnames.Any())
            {
                var name = GetRandomElement(names);
                var surname = GetRandomElement(surnames);
                var email = StringHelper.RemoveAccents($"{name}.{surname}@zzz.zzz").ToLower();
                var pass = "Pass@123";

                var user = new User(email, name, surname, pass)
                {
                    Roles = GetRandomElements(roles, random.Next(1, 5)),
                    Permissions = GetRandomElements(permissions, random.Next(3, 8))
                };

                names.Remove(name);
                surnames.Remove(surname);

                users.Add(user);
            }

            //create System Users
            users.Add(new User("admin@zzz.zzz", "Admin", "System", "Admin@123") { Type = UserType.Admin });
            users.Add(new User("super@zzz.zzz", "Super", "System", "Super@123") { Type = UserType.SuperAdmin });

            dbContext.Users.AddRange(users);

            if (dbContext.SaveChanges() > 0)
                logger.LogInformation("SeedUsers completed successfully.");
        }

        private static T GetRandomElement<T>(ICollection<T> collection)
        {
            return GetRandomElements(collection, 1).Single();
        }
        private static ICollection<T> GetRandomElements<T>(ICollection<T> collection, int count)
        {
            if (!collection.Any())
                throw new ArgumentException("The collection is null or empty.");

            if (count < 1 || count > collection.Count)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be between 1 and the collection's count.");

            var random = new Random();
            return collection.OrderBy(_ => random.Next()).Take(count).ToList();
        }
    }
}
