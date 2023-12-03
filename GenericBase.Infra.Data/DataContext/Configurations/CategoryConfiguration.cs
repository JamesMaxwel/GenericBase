using GenericBase.Domain.Entities;
using GenericBase.Infra.Data.DataContext.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericBase.Infra.Data.DataContext.Configurations
{
    public class CategoryConfiguration : BaseConfiguration<ProductCategory>
    {
        public override void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            base.Configure(builder);

        }
    }
}
