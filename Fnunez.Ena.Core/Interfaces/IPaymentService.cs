using Fnunez.Ena.Core.Entities;

namespace Fnunez.Ena.Core.Interfaces;

public interface IPaymentService
{
    Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
}