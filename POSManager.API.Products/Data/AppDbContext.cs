using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using POSManager.API.Products.Data.Configuration;
using POSManager.API.Products.Models;

namespace POSManager.API.Products.Data;

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

        var connString = ((CustomTenant)TenantInfo).ConnectionStringProducts;
        optionsBuilder.UseSqlServer(connString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureMultiTenant();

        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());



        var caregorykey = modelBuilder.Entity<Category>().Metadata.GetKeys().First();

        modelBuilder.Entity<Category>().IsMultiTenant()
            //.AdjustKey(caregorykey, modelBuilder)  //This causes all sorts of issues, several listed on the Finbuckle repo.
            .AdjustIndexes();

        var productkey = modelBuilder.Entity<Product>().Metadata.GetKeys().First();

        modelBuilder.Entity<Product>().IsMultiTenant()
            //.AdjustKey(productkey, modelBuilder) //This causes all sorts of issues, several listed on the Finbuckle repo.
            .AdjustIndexes();

    }

    public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
    {
        this.EnforceMultiTenant();

        return base.AddAsync(entity, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.EnforceMultiTenant();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        this.EnforceMultiTenant();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }


}
