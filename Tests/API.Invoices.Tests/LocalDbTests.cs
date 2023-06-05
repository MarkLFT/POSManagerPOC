using API.Invoices.Tests.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace API.Invoices.Tests;
public class LocalDbTests : IDisposable
{
    private const string InMemoryConnectionString = "DataSource=:memory:";
    private readonly SqliteConnection _connection;

    protected readonly LocalDbContext _dbContext;

    public LocalDbTests()
    {
        _connection = new SqliteConnection(InMemoryConnectionString);
        _connection.Open();

        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseSqlite(_connection)
            .Options;

        _dbContext = new LocalDbContext(options);
        _dbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task create_invoice_with_no_items()
    {
        var invoice = GetInvoiceNoItems();

        var addResult = _dbContext.Invoices.Add(invoice);

        Assert.NotNull(addResult);
        Assert.True(addResult.State == Microsoft.EntityFrameworkCore.EntityState.Added);
        Assert.True(addResult.Entity.InvoiceId == invoice.InvoiceId);

        Assert.True(addResult.Entity.InvoiceItems.Any() == false);

        var saveResult = _dbContext.SaveChanges();
        Assert.True(saveResult == 1);

    }

    [Fact]
    public async Task create_invoice_with_items()
    {
        var invoice = GetInvoiceWithItems();

        var addResult = _dbContext.Invoices.Add(invoice);

        Assert.NotNull(addResult);
        Assert.True(addResult.State == Microsoft.EntityFrameworkCore.EntityState.Added);
        Assert.True(addResult.Entity.InvoiceId == invoice.InvoiceId);

        Assert.True(addResult.Entity.InvoiceItems.Any() == true);
        Assert.True(addResult.Entity.InvoiceItems.Count == 2);

        var saveResult = _dbContext.SaveChanges();
        Assert.True(saveResult == 3);
    }

    [Fact]
    public async Task get_invoices_with_items() 
    {
        await PrefillInvoices();

        var invoices = await _dbContext.Invoices.ToListAsync();

        Assert.NotNull(invoices);
        Assert.True(invoices.Any());
        Assert.True(invoices.All(z => z.InvoiceItems.Any() == true));
    }

    private static InvoiceEF GetInvoiceNoItems()
    {
        var invoice = new InvoiceEF()
        {
            InvoiceId = Guid.NewGuid(),
            GuestName = "Mark",
            InvoiceNo = "BC0001",
            IssuedDate = DateTime.Now,
            InvoiceItems = new List<InvoiceItemEF>()
        };

        return invoice;
    }

    private static InvoiceEF GetInvoiceWithItems()
    {
        var id = Guid.NewGuid();

        var invoice = new InvoiceEF()
        {
            InvoiceId = id,
            GuestName = "Mark",
            InvoiceNo = "BC0001",
            IssuedDate = DateTime.Now,
            InvoiceItems = new List<InvoiceItemEF>
            {
                new InvoiceItemEF
                {
                    InvoiceId = id,
                    InvoiceItemId = Guid.NewGuid(),
                    Description = "Item 1",
                    LineNumber = 1,
                    Price = 10M,
                    ProductId = 100,
                    Quantity = 1
                },
                new InvoiceItemEF
                {
                    InvoiceId = id,
                    InvoiceItemId = Guid.NewGuid(),
                    Description = "Item 2",
                    LineNumber = 2,
                    Price = 15M,
                    ProductId = 101,
                    Quantity = 2
                }
            }
        };

        return invoice;
    }

    private async Task PrefillInvoices()
    {
        var invoice = GetInvoiceWithItems();

        await _dbContext.Invoices.AddAsync(invoice);

        invoice = GetInvoiceWithItems();

        await _dbContext.Invoices.AddAsync(invoice);

        invoice = GetInvoiceWithItems();

        await _dbContext.Invoices.AddAsync(invoice);

        await _dbContext.SaveChangesAsync();


    }

    public void Dispose()
    {
        _connection.Close();
    }
}
