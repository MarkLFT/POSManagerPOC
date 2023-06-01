using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using POSManager.API.Products.Models;

namespace POSManager.API.Products.Data;

public class SharedDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddUserSecrets("8c9250cf-7d00-4524-9633-95a8bc67bb67")
            .Build();

        var tenantInfo = new CustomTenant { ConnectionStringProducts = configuration.GetConnectionString("Products") };
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        return new AppDbContext(tenantInfo, optionsBuilder.Options);
    }
}

