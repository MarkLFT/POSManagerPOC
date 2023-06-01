using NubeSync.Core;
using NubeSync.Server;
using NubeSync.Server.Data;
using Microsoft.AspNetCore.Mvc;
using POSManager.API.Invoices.Data;

namespace POSManager.API.Invoices.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperationsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IOperationService _operationService;

    public OperationsController(
        AppDbContext context,
        IOperationService operationService)
    {
        _context = context;
        _operationService = operationService;
    }

    [HttpPost]
    public async Task<IActionResult> PostOperationsAsync(List<NubeOperation> operations)
    {
        try
        {
            await _operationService.ProcessOperationsAsync(_context, operations);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }
}
