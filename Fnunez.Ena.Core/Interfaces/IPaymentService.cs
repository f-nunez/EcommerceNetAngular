using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Entities.OrderAggregate;

namespace Fnunez.Ena.Core.Interfaces;

public interface IPaymentService
{
    Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
    Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId);
}