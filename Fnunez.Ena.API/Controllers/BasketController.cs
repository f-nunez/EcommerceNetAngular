using AutoMapper;
using Fnunez.Ena.API.Dtos;
using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fnunez.Ena.API.Controllers;

public class BasketController : BaseApiController
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;

    public BasketController(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    [HttpDelete]
    public async Task DeleteBasket(string id)
    {
        await _basketRepository.DeleteBasketAsync(id);
    }

    [HttpGet]
    public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
    {
        CustomerBasket basket = await _basketRepository.GetBasketAsync(id);

        return Ok(basket ?? new CustomerBasket(id));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto customerBasketDto)
    {
        CustomerBasket customerBasket = _mapper
            .Map<CustomerBasketDto, CustomerBasket>(customerBasketDto);

        CustomerBasket updatedBasket = await _basketRepository
            .UpdateBasketAsync(customerBasket);

        return Ok(updatedBasket);
    }
}