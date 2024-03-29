using Fnunez.Ena.Core.Entities;

namespace Fnunez.Ena.Core.Specifications;

public class ProductsWithFiltersForCountSpecification : BaseSpecifcation<Product>
{
    public ProductsWithFiltersForCountSpecification(ProductSpecParams productSpecParams)
        : base(x =>
            (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(productSpecParams.Search)) &&
            (!productSpecParams.BrandId.HasValue || x.ProductBrandId == productSpecParams.BrandId) &&
            (!productSpecParams.TypeId.HasValue || x.ProductTypeId == productSpecParams.TypeId)
        )
    {
    }
}