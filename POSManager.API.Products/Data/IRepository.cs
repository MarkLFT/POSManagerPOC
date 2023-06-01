using POSManager.API.Products.Models;

namespace POSManager.API.Products.Data;

public interface IRepository
{
    #region Categories

    Task<bool> AddUpdateCategory(Category category);
    Task<bool> DeleteCategory(Guid id);
    Task<IEnumerable<Category>> GetCategoriesAsync(bool withProducts = false);
    Task<Category?> GetCategoryAsync(Guid id);

    #endregion Categories

    #region Products

    Task<bool> AddUpdateProduct(Product product);
    Task<bool> DeleteProduct(Guid id);
    Task<Product?> GetProductAsync(Guid id);
    Task<IEnumerable<Product>> GetProductsAsync();

    #endregion Products
}
