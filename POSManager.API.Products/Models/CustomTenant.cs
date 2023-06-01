using Finbuckle.MultiTenant;

namespace POSManager.API.Products.Models;

public class CustomTenant : ITenantInfo
{
    public string? Id { get; set; }
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public string? ConnectionString { get; set; }
    public string? ConnectionStringProducts { get; set; }
    public string? ConnectionStringInvoices { get; set; }
}
