using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POSManager.API.Invoices.Models;
using POSManager.API.Invoices.Data.Configuration;
using NubeSync.Server.Data;

namespace POSManager.API.Invoices.Data;

public class AppDbContext : DbContext, IMultiTenantDbContext, IAppDbContext
{
    public ITenantInfo TenantInfo { get; }

    public TenantMismatchMode TenantMismatchMode { get; } 

    public TenantNotSetMode TenantNotSetMode { get; } 

    public AppDbContext(CustomTenant tenantInfo) 
    {
        TenantInfo = tenantInfo;
    }

    public AppDbContext(CustomTenant tenantInfo, DbContextOptions<AppDbContext> options) : base(options)
    {
        TenantInfo = tenantInfo;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (TenantInfo is null)
            throw new NullReferenceException(nameof(TenantInfo));

        var connString = ((CustomTenant)TenantInfo).ConnectionStringInvoices;

        optionsBuilder.UseSqlServer(connString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureMultiTenant();

        modelBuilder.ApplyConfiguration(new InvoiceItemConfiguration());
        modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        


        //var invoiceKey = modelBuilder.Entity<Invoice>().Metadata.GetKeys().First();

        modelBuilder.Entity<Invoice>().IsMultiTenant()
            //.AdjustKey(invoiceKey, modelBuilder) //This causes all sorts of issues, several listed on the Finbuckle repo.
            .AdjustIndexes();

        //var invoiceItemKey = modelBuilder.Entity<InvoiceItem>().Metadata.GetKeys().First();

        modelBuilder.Entity<InvoiceItem>().IsMultiTenant()
            //.AdjustKey(invoiceItemKey, modelBuilder)  //This causes all sorts of issues, several listed on the Finbuckle repo.
            .AdjustIndexes();

    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.EnforceMultiTenant();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        this.EnforceMultiTenant();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    public DbSet<NubeServerOperation> Operations { get; set; }
}
