using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.DataContext.Configurations.Common;
using GenericBase.Infra.Data.DataContext.EntityRelations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericBase.Infra.Data.DataContext.Configurations
{
    public class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            //builder.ToTable("users");
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.Email).HasColumnType("varchar(50)").IsRequired();
            builder.Property(x => x.FirstName).HasColumnType("varchar(50)");
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(1024);



            builder.HasMany(x => x.Permissions)
             .WithMany(x => x.Users)
             .UsingEntity<UserVsPermission>(
                 tb1 => tb1.HasOne(p => p.Permission).WithMany().HasForeignKey(p => p.PermissionId),
                 tb2 => tb2.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId),
                 tbRelation => tbRelation/*.ToTable("_users_vs_permissions")*/.HasKey(p => new { p.UserId, p.PermissionId })
             );

            builder.HasMany(x => x.Roles)
             .WithMany(x => x.Users)
             .UsingEntity<UserVsRole>(
                 tb1 => tb1.HasOne(p => p.Role).WithMany().HasForeignKey(p => p.RoleId),
                 tb2 => tb2.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId),
                 tbRelation => tbRelation/*.ToTable("_users_vs_roles")*/.HasKey(p => new { p.UserId, p.RoleId })
             );

        }
    }
}
