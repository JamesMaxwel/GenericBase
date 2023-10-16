using GenericBase.Domain.Entities.Account;
using GenericBase.Infra.Data.DataContext.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericBase.Infra.Data.DataContext.Configurations
{
    public class PermissionConfiguration : BaseConfiguration<Permission>
    {
        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            base.Configure(builder);
            //builder.ToTable("permissions");
            builder.HasIndex(x => new { x.Key, x.Value }).IsUnique();

        }
    }
}
