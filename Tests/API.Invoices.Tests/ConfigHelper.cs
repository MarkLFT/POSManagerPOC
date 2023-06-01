using Microsoft.Extensions.Configuration;

namespace API.Invoices.Tests;

internal partial class ConfigHelper
{
    public IConfiguration? Configuration { get; set; }

    public ConfigHelper()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets("ed7ce8cc-1154-43df-a052-7acc40f42846")
            .AddEnvironmentVariables()
            .Build();
    }
}