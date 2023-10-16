using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.DataContext.Configurations.Common;
using GenericBase.Infra.Data.DataContext.EntityRelations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericBase.Infra.Data.DataContext.Configurations
{
    public class RoleConfiguration : BaseConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);
            //builder.ToTable("roles");
            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasMany(x => x.Permissions)
            .WithMany(x => x.Roles)
            .UsingEntity<RoleVsPermission>(
                tb1 => tb1.HasOne(p => p.Permission).WithMany().HasForeignKey(p => p.PermissionId),
                tb2 => tb2.HasOne(p => p.Role).WithMany().HasForeignKey(p => p.RoleId),
                tbRelation => tbRelation.ToTable("_roles_vs_permissions").HasKey(p => new { p.RoleId, p.PermissionId })
            );

        }
    }
}
