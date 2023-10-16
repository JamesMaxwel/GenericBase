using GenericBase.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GenericBase.Infra.Data.DataContext.Configurations.Common
{
    public abstract class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IBase
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
