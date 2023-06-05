using API.Invoices.Tests.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Invoices.Tests;
public class LocalDbContext : DbContext
{
    public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

    public LocalDbContext() { }

    public DbSet<InvoiceEF> Invoices => Set<InvoiceEF>();
    public DbSet<InvoiceItemEF> InvoiceItems => Set<InvoiceItemEF>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("InvoiceDB.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvoiceEF>().ToTable("Invoices").HasKey(x => x.InvoiceId);
        modelBuilder.Entity<InvoiceItemEF>().ToTable("InvoiceItems").HasKey(x => x.InvoiceItemId);
    }
}
