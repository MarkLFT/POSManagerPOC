using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSManager.API.Invoices.Models;

namespace POSManager.API.Invoices.Data.Configuration;

public class InvoiceItemConfiguration : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.ToTable("InvoiceItem", "dbo");

        builder.HasKey(x => x.InvoiceItemId);

        builder.Property(x => x.InvoiceId).HasColumnName(@"InvoiceId").HasColumnType("uniqueidentifier").IsRequired();
        builder.Property(x => x.LineNumber).HasColumnName(@"LineNumber").HasColumnType("int").IsRequired();
        builder.Property(x => x.ProductId).HasColumnName(@"ProductId").HasColumnType("int");
        builder.Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
        builder.Property(x => x.Price).HasColumnName(@"Price").HasColumnType("money").HasDefaultValue(0m);
        builder.Property(x => x.Quantity).HasColumnName(@"Quantity").HasColumnType("int").IsRequired().HasDefaultValue(0);
        
        builder.HasOne(a => a.Invoice).WithMany(b => b.InvoiceItems).HasForeignKey(c => c.InvoiceId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("FK_InvoiceItem_Invoice");
    }
}
