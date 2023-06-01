using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POSManager.API.Products.Data;
using POSManager.API.Products.Models;

namespace POSManager.API.Products.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IRepository _repository;

    public ProductController(ILogger<ProductController> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("/products", Name = "GetProducts")]
    [Authorize("products.read")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return Ok(await _repository.GetProductsAsync());
    }

    [HttpGet("/products/{id:Guid}", Name = "GetProduct")]
    [Authorize("products.read")]
    public async Task<ActionResult<Product>> GetProduct(Guid id)
    {
        return Ok(await _repository.GetProductAsync(id));
    }


    [HttpPost("/products", Name = "AddProduct")]
    [AuthorizeAttribute()]
    public async Task<ActionResult> Post(Product product)
    {
        if (product == null)
        {
            return BadRequest();
        }

        var result = await _repository.AddUpdateProduct(product);

        return result ? CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product)
                      : BadRequest();
    }

    [HttpPut("/products", Name = "EditProduct")]
    [AuthorizeAttribute()]
    public async Task<ActionResult> Put(Product product)
    {
        if (product == null)
        {
            return BadRequest();
        }

        var result = await _repository.AddUpdateProduct(product);

        return result ? Ok(product)
                      : BadRequest();
    }

    [HttpDelete("/products/{id:Guid}", Name = "DeleteProduct")]
    [AuthorizeAttribute()]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _repository.DeleteProduct(id);

        return result ? Ok() : BadRequest();
    }
}
