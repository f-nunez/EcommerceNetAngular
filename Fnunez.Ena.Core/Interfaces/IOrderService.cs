using Fnunez.Ena.Core.Entities.OrderAggregate;

namespace Fnunez.Ena.Core.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress);
    Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    Task<Order> GetOrderAsync(int id, string buyerEmail);
    Task<IReadOnlyList<Order>> GetOrdersAsync(string buyerEmail);
}