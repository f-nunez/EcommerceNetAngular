using Fnunez.Ena.Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fnunez.Ena.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(i => i.ProductItemOrdered, io =>
        {
            io.WithOwner();
        });

        builder.Property(i => i.Price)
            .HasPrecision(18, 2);
    }
}
