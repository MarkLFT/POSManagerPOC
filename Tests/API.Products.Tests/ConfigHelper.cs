using Microsoft.Extensions.Configuration;

namespace API.Products.Tests;
internal partial class ConfigHelper
{
    public IConfiguration? Configuration { get; set; }

    public ConfigHelper()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets("30fc2bae-b2a2-424c-86d1-06381c2d3dfa")
            .AddEnvironmentVariables()
            .Build();
    }
}
