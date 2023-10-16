using GenericBase.Domain.Entities;
using GenericBase.Infra.Data.DataContext.Configurations.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericBase.Infra.Data.DataContext.Configurations
{
    public class CustomerConfiguration : BaseConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);
            //builder.ToTable("customers");

        }
    }
}
