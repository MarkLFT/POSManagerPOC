using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using POSManager.API.Products.Models;

namespace POSManager.API.Products.Data;
public interface IAppDbContext
{
    DbSet<Category> Categories { get; set; }
    DbSet<Product> Products { get; set; }
    ITenantInfo TenantInfo { get; }
    TenantMismatchMode TenantMismatchMode { get; }
    TenantNotSetMode TenantNotSetMode { get; }

    int SaveChanges(bool acceptAllChangesOnSuccess);
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
}