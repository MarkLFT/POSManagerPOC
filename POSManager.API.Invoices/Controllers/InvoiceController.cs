using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSManager.API.Invoices.Data;
using POSManager.API.Invoices.Models;

namespace POSManager.API.Invoices.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class InvoiceController : ControllerBase
{
    private readonly ILogger<InvoiceController> _logger;
    private readonly AppDbContext _dbContext;

    public InvoiceController(ILogger<InvoiceController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    [Authorize("invoices.read")]
    public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices(DateTimeOffset? laterThan)
    {
        var tableName = typeof(Invoice).Name;

        if (laterThan.HasValue)
        {
            return await _dbContext.Invoices.Where(i => i.ServerUpdatedAt >= laterThan).ToListAsync();
        }
        else
        {
            return await _dbContext.Invoices.Where(i => !i.DeletedAt.HasValue).ToListAsync();
        }
    }
}
