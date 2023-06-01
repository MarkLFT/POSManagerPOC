namespace POSManager.API.Products.Models;

public class Product
{
    public Guid ProductId { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }

    public Guid CategoryId { get; set; }

}
