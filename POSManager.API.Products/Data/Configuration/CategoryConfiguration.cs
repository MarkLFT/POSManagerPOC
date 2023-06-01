using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSManager.API.Products.Models;

namespace POSManager.API.Products.Data.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category", "dbo");

        builder.HasKey(x => x.CategoryId);

        builder.Property(x => x.Code).HasColumnName(@"Code").HasColumnType("nvarchar(5)").IsRequired().HasMaxLength(5);
        builder.Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(25)").IsRequired().HasMaxLength(25);

        builder.HasMany(x => x.Products);
    }
}
