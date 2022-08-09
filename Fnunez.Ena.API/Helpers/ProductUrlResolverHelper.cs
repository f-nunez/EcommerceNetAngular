using AutoMapper;
using Fnunez.Ena.API.Dtos;
using Fnunez.Ena.Core.Entities;

namespace Fnunez.Ena.API.Helpers;

public class ProductUrlResolverHelper : IValueResolver<Product, ProductToReturnDto, string>
{
    private readonly IConfiguration _config;

    public ProductUrlResolverHelper(IConfiguration config)
    {
        _config = config;
    }

    public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
            return _config["ApiUrl"] + source.PictureUrl;
            
        return null;
    }
}
