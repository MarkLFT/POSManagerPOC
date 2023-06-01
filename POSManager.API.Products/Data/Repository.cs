using Microsoft.EntityFrameworkCore;
using POSManager.API.Products.Models;

namespace POSManager.API.Products.Data;

public class Repository : IRepository
{
    private readonly AppDbContext _dbContext;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region Categories

    public async Task<Category?> GetCategoryAsync(Guid id)
    {
        return await _dbContext.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.CategoryId == id);
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync(bool withProducts = false)
    {
        return withProducts ? await _dbContext.Categories.AsNoTracking()
                                              .Include(x => x.Products).AsNoTracking()
                                    .ToListAsync()
                            : await _dbContext.Categories.AsNoTracking().ToListAsync();
;
    }

    public async Task<bool> AddUpdateCategory(Category category)
    {
        var existingCategory = await _dbContext.Categories.SingleOrDefaultAsync(x => x.CategoryId == category.CategoryId);

        if (existingCategory is null)
        {
            existingCategory = category;

            await _dbContext.Categories.AddAsync(existingCategory);
        }

        existingCategory.Description = category.Description;
        existingCategory.Code = category.Code;

        foreach (var product in existingCategory.Products)
        {
            var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if (existingProduct is null)
                continue;

            _dbContext.Remove(existingProduct);
        }

        foreach (var product in category.Products)
        {
            var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if (existingProduct is null)
            {
                existingProduct = new Product { ProductId = product.ProductId, Code = product.Code, Description = product.Description, CategoryId = product.CategoryId };
                await _dbContext.Products.AddAsync(existingProduct);
            }

            existingProduct.Code = product.Code;
            existingProduct.Description = product.Description;

        }

        var result = await _dbContext.SaveChangesAsync();

        _dbContext.Entry(existingCategory).State = EntityState.Detached;

        return result > 0;
    }

    public async Task<bool> DeleteCategory(Guid id)
    {
        var existingCategory = await _dbContext.Categories.SingleOrDefaultAsync(x => x.CategoryId == id);

        if (existingCategory is null)
            return false;

        _dbContext.Remove(existingCategory);

        var result = await _dbContext.SaveChangesAsync();

        _dbContext.Entry(existingCategory).State = EntityState.Detached;

        return result > 0;

    }

    #endregion Categories

    #region Products

    public async Task<Product?> GetProductAsync(Guid id)
    {
        return await _dbContext.Products.AsNoTracking().SingleOrDefaultAsync(x => x.ProductId == id);
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _dbContext.Products.AsNoTracking().ToListAsync();
    }

    public async Task<bool> AddUpdateProduct(Product product)
    {
        var existingProduct = await _dbContext.Products.SingleOrDefaultAsync(x => x.ProductId == product.ProductId);

        if (existingProduct is null)
        {
            existingProduct = product;

            await _dbContext.Products.AddAsync(existingProduct);
        }

        existingProduct.Description = product.Description;
        existingProduct.Code = product.Code;
        existingProduct.CategoryId = product.CategoryId;

        var result = await _dbContext.SaveChangesAsync();

        _dbContext.Entry(existingProduct).State = EntityState.Detached;

        return result > 0;
    }

    public async Task<bool> DeleteProduct(Guid id)
    {
        var existingProduct= await _dbContext.Products.SingleOrDefaultAsync(x => x.ProductId == id);

        if (existingProduct is null)
            return false;

        _dbContext.Remove(existingProduct);

        var result = await _dbContext.SaveChangesAsync();

        _dbContext.Entry(existingProduct).State = EntityState.Detached;

        return result > 0;

    }



    #endregion Products


}
