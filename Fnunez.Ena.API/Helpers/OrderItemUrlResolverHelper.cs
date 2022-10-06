using AutoMapper;
using Fnunez.Ena.API.Dtos;
using Fnunez.Ena.Core.Entities.OrderAggregate;

namespace Fnunez.Ena.API.Helpers;

public class OrderItemUrlResolverHelper : IValueResolver<OrderItem, OrderItemDto, string>
{
    private readonly IConfiguration _configuration;

    public OrderItemUrlResolverHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.ProductItemOrdered.PictureUrl))
            return _configuration["ApiUrl"] + source.ProductItemOrdered.PictureUrl;

        return null;
    }
}