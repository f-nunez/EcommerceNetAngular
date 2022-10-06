using System.Net;
using AutoMapper;
using Fnunez.Ena.API.Dtos;
using Fnunez.Ena.API.Errors;
using Fnunez.Ena.API.Extensions;
using Fnunez.Ena.Core.Entities.OrderAggregate;
using Fnunez.Ena.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fnunez.Ena.API.Controllers;

[Authorize]
public class OrderController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrderController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost("createorder")]
    public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
    {
        string email = HttpContext.User.RetrieveEmailFromClaimsPrincipal();

        Address address = _mapper.Map<AddressDto, Address>(orderDto.ShippingToAddress);

        Order order = await _orderService
            .CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

        if (order != null)
            return Ok(order);

        return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, "Problem creating the order"));
    }

    [HttpGet("getdeliverymethods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {
        return Ok(await _orderService.GetDeliveryMethodsAsync());
    }

    [HttpGet("getorder/{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        string email = HttpContext.User.RetrieveEmailFromClaimsPrincipal();

        Order order = await _orderService.GetOrderAsync(id, email);

        if (order == null)
            return NotFound(new ApiResponse((int)HttpStatusCode.NotFound));

        OrderToReturnDto orderToReturnDto = _mapper.Map<Order, OrderToReturnDto>(order);
        return Ok(orderToReturnDto);

    }

    [HttpGet("getorders")]
    public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrders()
    {
        string email = HttpContext.User.RetrieveEmailFromClaimsPrincipal();

        IReadOnlyList<Order> orders = await _orderService.GetOrdersAsync(email);

        IReadOnlyList<OrderToReturnDto> OrderDtos = _mapper
            .Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders);

        return Ok(OrderDtos);
    }
}