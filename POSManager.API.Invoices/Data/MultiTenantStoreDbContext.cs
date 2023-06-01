using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;
using POSManager.API.Invoices.Models;

namespace POSManager.API.Invoices.Data;

public class MultiTenantStoreDbContext : EFCoreStoreDbContext<CustomTenant>
{
    public MultiTenantStoreDbContext(DbContextOptions<MultiTenantStoreDbContext> options) : base(options)
    {
      
    }
}
