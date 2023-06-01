using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;

namespace POSManager.API.Tenant.Controllers;
[Route("[controller]")]
[ApiController]
public class TenantsController : ControllerBase
{
    private readonly ILogger<TenantsController> _logger;
    private readonly IMultiTenantStore<TenantInfo> store;

    public TenantsController(ILogger<TenantsController> logger, IMultiTenantStore<TenantInfo> store)
    {
        _logger = logger;
        this.store = store;
    }

    [HttpGet("{identifier}",Name ="GetTenant")]
    public async Task<ActionResult<TenantInfo>> Get(string identifier)
    {
        _logger.LogInformation("Tenants endpoint called with identifier \"{identifier}\".", identifier);

        var tenantInfo = await store.TryGetByIdentifierAsync(identifier);
        if (tenantInfo != null)
        {
            _logger.LogInformation("Tenant \"{name}\" found for identifier \"{identifier}\".", tenantInfo.Name, identifier);
            return tenantInfo;
        }

        _logger.LogWarning("No tenant found with identifier \"{identifier}\".", identifier);
        return NotFound();
    }

    //[HttpGet(Name = "GetAll")]
    //public async Task<ActionResult<IEnumerable<TenantInfo>>> Get()
    //{
    //    _logger.LogInformation("Tenants endpoint called with for all tenants");

    //    var tenantInfo = await store.GetAllAsync();
    //    if (tenantInfo != null)
    //    {
    //       // _logger.LogInformation("Tenant \"{name}\" found for identifier \"{identifier}\".", tenantInfo.Name, identifier);
    //        return Ok(tenantInfo);
    //    }

    //    _logger.LogWarning("No tenants found ");
    //    return NotFound();
    //}
}
