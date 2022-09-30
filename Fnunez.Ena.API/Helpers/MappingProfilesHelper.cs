using AutoMapper;
using Fnunez.Ena.API.Dtos;
using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Entities.Identity;

namespace Fnunez.Ena.API.Helpers;

public class MappingProfilesHelper : Profile
{
    public MappingProfilesHelper()
    {
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(d => d.ProductBrand, m => m.MapFrom(x => x.ProductBrand.Name))
            .ForMember(d => d.ProductType, m => m.MapFrom(x => x.ProductType.Name))
            .ForMember(d => d.PictureUrl, m => m.MapFrom<ProductUrlResolverHelper>());

        CreateMap<Address, AddressDto>().ReverseMap();
    }
}