using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using POSManager.API.Invoices.Data;
using POSManager.API.Invoices.Models;

namespace POSManager.API.Products.Data;

public class SharedDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddUserSecrets("8cd743db-f1fb-4799-ad68-81ea7bfabb5a")
            .Build();

        var tenantInfo = new CustomTenant { ConnectionStringInvoices = configuration.GetConnectionString("Invoices") };
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        return new AppDbContext(tenantInfo, optionsBuilder.Options);
    }
}

