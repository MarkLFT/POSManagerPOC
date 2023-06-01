namespace POSManager.API.Products.Models;

public class Category
{
    public Guid CategoryId { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }

    public IEnumerable<Product> Products { get; set;} = new List<Product>();

}
