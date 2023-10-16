using GenericBase.Domain.Entities;
using GenericBase.Infra.Data.DataContext.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericBase.Infra.Data.DataContext.Configurations
{
    public class ProductConfiguration : BaseConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);
            //builder.ToTable("products");

        }
    }
}
