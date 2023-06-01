using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSManager.API.Products.Models;

namespace POSManager.API.Products.Data.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product", "dbo");

        builder.HasKey(x => x.ProductId);

        builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar(5)").IsRequired().HasMaxLength(5);
        builder.Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
    }
}
