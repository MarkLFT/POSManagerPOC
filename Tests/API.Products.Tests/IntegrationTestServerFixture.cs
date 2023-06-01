using Microsoft.Extensions.Configuration;
using POSManager.API.Products;

namespace API.Products.Tests;

public class IntegrationTestServerFixture : IDisposable, IClassFixture<TestWebApplicationFactory<Program>>
{
    public IntegrationTestServerFixture() 
    {
        var configHelper = new ConfigHelper();

        if (configHelper.Configuration is null)
            throw new NullReferenceException(nameof(configHelper));

        _configuration = configHelper.Configuration;

        _factory = new TestWebApplicationFactory<Program>();

        var tokenDemo = Helpers.GetAuthCode("m2m.client.demo", "511536EF-F270-4058-80CA-1C89C192F69A", _configuration.GetValue<string>("Authentication:Schemes:Bearer:Authority")).Result;

        HttpClientDemo = _factory.CreateClient();

        if (tokenDemo is not null)
            HttpClientDemo.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenDemo);

        var tokenJoe = Helpers.GetAuthCode("m2m.client.joe", "511536EF-F270-4058-80CA-1C89C192F69A", _configuration.GetValue<string>("Authentication:Schemes:Bearer:Authority")).Result;

        HttpClientJoe = _factory.CreateClient();

        if (tokenJoe is not null)
            HttpClientJoe.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenJoe);
    }

    public void Dispose()
    {
        HttpClientDemo.Dispose();
        _factory.Dispose();
    }

    private readonly TestWebApplicationFactory<Program> _factory;
    public HttpClient HttpClientDemo { get; set; }
    public HttpClient HttpClientJoe { get; set; }
    private IConfiguration _configuration;
}

