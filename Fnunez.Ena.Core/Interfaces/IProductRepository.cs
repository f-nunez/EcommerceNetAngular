using Fnunez.Ena.Core.Entities;

namespace Fnunez.Ena.Core.Interfaces;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(int id);
    Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
    Task<IReadOnlyList<Product>> GetProductsAsync();
    Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
}