using Fnunez.Ena.Core.Entities;

namespace Fnunez.Ena.Core.Interfaces;

public interface IBasketRepository
{
    Task<bool> DeleteBasketAsync(string basketId);
    Task<CustomerBasket> GetBasketAsync(string basketId);
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
}