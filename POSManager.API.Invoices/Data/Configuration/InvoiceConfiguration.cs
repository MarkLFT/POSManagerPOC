using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using POSManager.API.Invoices.Models;

namespace POSManager.API.Invoices.Data.Configuration;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoice", "dbo");

        builder.HasKey(x => x.InvoiceId);

        builder.Property(x => x.InvoiceNo).HasColumnName(@"InvoiceNo").HasColumnType("nvarchar(10)").IsRequired().HasMaxLength(10);
        builder.Property(x => x.GuestName).HasColumnName(@"GuestName").HasColumnType("nvarchar(50)").IsRequired().HasMaxLength(50);
        builder.Property(x => x.IssuedDate).HasColumnName(@"IssuedDate").HasColumnType("smalldatetime").IsRequired().HasDefaultValue(DateTime.Now);
        builder.Property(x => x.TableId).HasColumnName(@"TableId").HasColumnType("int");

        builder.HasMany(x => x.InvoiceItems);
    }
}
