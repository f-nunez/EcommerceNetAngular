using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fnunez.Ena.Infrastructure.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly StoreDbContext _dbContext;

    public ProductRepository(StoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
    {
        return await _dbContext.ProductBrands.ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _dbContext.Products
            .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        return await _dbContext.Products
            .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
    {
        return await _dbContext.ProductTypes.ToListAsync();
    }
}