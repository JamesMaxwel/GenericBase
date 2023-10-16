using GenericBase.Domain.Entities;
using GenericBase.Infra.Data.DataContext.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericBase.Infra.Data.DataContext.Configurations
{
    public class OrderDetailConfiguration : BaseConfiguration<OrderItem>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            base.Configure(builder);
            //builder.ToTable("order_details");

        }
    }
}
