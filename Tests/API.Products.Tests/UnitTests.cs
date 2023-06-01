using POSManager.API.Products.Data;
using POSManager.API.Products.Models;

namespace API.Products.Tests;
public class UnitTests
{

    [Fact]
    public async Task AddCategory()
    {
        var repo = new Repository(new AppDbContext(new CustomTenant { Id = "demo", Identifier = "demo", Name = "Demo Store", ConnectionStringProducts = "Data Source=db.rmserver.local;Initial Catalog=CashierProducts;User ID=sa;Password=Nimda1808;MultipleActiveResultSets=True;Encrypt=false;TrustServerCertificate=true" }));

        var result = await repo.AddUpdateCategory(new Category { CategoryId = Guid.Parse("201e8c7f-225f-4e27-80fa-4c304e13ace1"), Code = "DM1", Description = "Demo Cat 1", Products = new List<Product>() });

        Assert.True(result);

        result = await repo.DeleteCategory(Guid.Parse("201e8c7f-225f-4e27-80fa-4c304e13ace1"));

        Assert.True(result);
    }


}
