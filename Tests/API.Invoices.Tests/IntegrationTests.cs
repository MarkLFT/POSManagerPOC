using NubeSync.Client.SQLiteStore;
using NubeSync.Client;
using API.Invoices.Tests.Models;

namespace API.Invoices.Tests;
public class IntegrationTest : IClassFixture<IntegrationTestServerFixture>
{
    private readonly IntegrationTestServerFixture _integrationTestServerFixture;
    private NubeSQLiteDataStore _dataStoreDemo;
    private NubeClient _nubeClientDemo;
    private IOrderedEnumerable<Invoice> _list;

    public IntegrationTest(IntegrationTestServerFixture integrationTestServerFixture)
    {
        _integrationTestServerFixture = integrationTestServerFixture;

        // this is needed when the server is running local for debugging
        //var clientHandler = new HttpClientHandler
        //{
        //    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        //};
        //var httpClient = new HttpClient(clientHandler);

        var server = "https://localhost:7002/";

        #region Demo client

        var databasePathDemo = Path.Combine(AppContext.BaseDirectory, "offlineDemo.db");
        _dataStoreDemo = new NubeSQLiteDataStore(databasePathDemo);

        _nubeClientDemo = new NubeClient(_dataStoreDemo, server, httpClient: _integrationTestServerFixture.HttpClientDemo);
        
        #endregion Demo Client


    }

    [Fact]
    public async Task add_invoice_to_local_store_and_sync()
    {
        await CreateTables();

        var invoiceId = Guid.Parse("c47069e8-93cd-4422-89a9-f1e3927c61c4");
        var issuedDate = DateTime.Now;

        var invoice = new Invoice
        {
            InvoiceId = invoiceId,
            InvoiceNo = "0001",
            GuestName = "Demo Guest",
            IssuedDate = issuedDate,
            InvoiceItems = new List<InvoiceItem>()
        };

        await _nubeClientDemo.SaveAsync(invoice);

        await SyncAsync(_nubeClientDemo);

        var demoServerInvoices = await GetInvoicesAsync(_integrationTestServerFixture.HttpClientDemo);

        var serverInvoice = demoServerInvoices.FirstOrDefault(x => x.InvoiceId == invoiceId);

        Assert.NotNull(serverInvoice);
        Assert.True(serverInvoice.IssuedDate == issuedDate);

    }

    private async Task RefreshItemsAsync(NubeClient nubeClient)
    {
        _list = (await nubeClient.GetAllAsync<Invoice>()).ToList().OrderBy(i => i.CreatedAt);
    }

    private async Task SyncAsync(NubeClient nubeClient)
    {
        await nubeClient.PushChangesAsync();
        await nubeClient.PullTableAsync<Invoice>();

        await RefreshItemsAsync(nubeClient);
    }

    private async Task<IEnumerable<POSManager.API.Invoices.Models.Invoice>> GetInvoicesAsync(HttpClient client)
    {
        return await Helpers.GetAsync<IEnumerable<POSManager.API.Invoices.Models.Invoice>>("invoices", client);
    }

    private async Task CreateTables()
    {
        await _dataStoreDemo.InitializeAsync();

        await _nubeClientDemo.AddTableAsync<InvoiceItem>("invoiceitem");
        await _nubeClientDemo.AddTableAsync<Invoice>("invoice");

        await RefreshItemsAsync(_nubeClientDemo);
    }
}
