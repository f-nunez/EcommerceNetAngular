using AutoMapper;
using Fnunez.Ena.API.Dtos;
using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Entities.OrderAggregate;

namespace Fnunez.Ena.API.Helpers;

public class MappingProfilesHelper : Profile
{
    public MappingProfilesHelper()
    {
        CreateMap<Product, ProductToReturnDto>()
            .ForMember(d => d.ProductBrand, m => m.MapFrom(x => x.ProductBrand.Name))
            .ForMember(d => d.ProductType, m => m.MapFrom(x => x.ProductType.Name))
            .ForMember(d => d.PictureUrl, m => m.MapFrom<ProductUrlResolverHelper>());

        CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

        CreateMap<CustomerBasketDto, CustomerBasket>();

        CreateMap<BasketItemDto, BasketItem>();

        CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();

        CreateMap<Order, OrderToReturnDto>()
            .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ProductItemOrdered.ProductItemId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductItemOrdered.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ProductItemOrdered.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolverHelper>());
    }
}