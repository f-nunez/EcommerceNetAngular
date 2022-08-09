using Fnunez.Ena.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fnunez.Ena.Infrasctructure.Data.Configurations;

public class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(pt => pt.Name)
            .IsRequired()
            .HasMaxLength(200);
    }
}