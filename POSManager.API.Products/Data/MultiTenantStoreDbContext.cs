using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using POSManager.API.Products.Models;

namespace POSManager.API.Products.Data;

public class MultiTenantStoreDbContext : EFCoreStoreDbContext<CustomTenant>
{
    public MultiTenantStoreDbContext(DbContextOptions<MultiTenantStoreDbContext> options) : base(options)
    {
      
    }
}
