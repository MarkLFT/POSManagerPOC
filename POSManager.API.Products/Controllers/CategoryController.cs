using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POSManager.API.Products.Data;
using POSManager.API.Products.Models;

namespace POSManager.API.Products.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly IRepository _repository;

    public CategoryController(ILogger<CategoryController> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet("/categories/{withProducts:bool?}", Name = "GetCategories")]
    [Authorize("products.read")]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories(bool withProducts = false)
    {
        return Ok(await _repository.GetCategoriesAsync(withProducts));
    }

    [HttpGet("/categories/{id:guid}", Name = "GetCategory")]
    [Authorize("products.read")]
    public async Task<ActionResult<Category>> GetCategory(Guid id)
    {
        return Ok(await _repository.GetCategoryAsync(id));
    }

    [HttpPost("/categories", Name = "AddCategory")]
    [AuthorizeAttribute()]
    public async Task<ActionResult<Category>> Post(Category category)
    {
        if (category == null) 
        {
            return BadRequest();
        }

        var result = await _repository.AddUpdateCategory(category);

        return result ? CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId} , category)
                      : BadRequest();
    }

    [HttpPut("/categories", Name = "EditCategory")]
    [AuthorizeAttribute()]
    public async Task<ActionResult> Put(Category category)
    {
        if (category == null)
        {
            return BadRequest();
        }

        var result = await _repository.AddUpdateCategory(category);

        return result ? Ok(category) 
                      : BadRequest();
    }

    [HttpDelete("/categories/{id:Guid}", Name = "DeleteCategory")]
    [AuthorizeAttribute()]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _repository.DeleteCategory(id);

        return result ? Ok()  : BadRequest();
    }
}
