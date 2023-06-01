using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using POSManager.API.Invoices.Models;

namespace POSManager.API.Invoices.Data;
public interface IAppDbContext
{
    DbSet<Invoice> Invoices { get; set; }
    DbSet<InvoiceItem> InvoiceItems { get; set; }
    ITenantInfo TenantInfo { get; }
    TenantMismatchMode TenantMismatchMode { get; }
    TenantNotSetMode TenantNotSetMode { get; }

    int SaveChanges(bool acceptAllChangesOnSuccess);
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
}