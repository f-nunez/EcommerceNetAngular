using Fnunez.Ena.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fnunez.Ena.Infrasctructure.Data.Configurations;

public class ProductBrandConfiguration : IEntityTypeConfiguration<ProductBrand>
{
    public void Configure(EntityTypeBuilder<ProductBrand> builder)
    {
        builder.HasKey(pb => pb.Id);

        builder.Property(pb => pb.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(pb => pb.Name)
            .IsRequired()
            .HasMaxLength(200);
    }
}
