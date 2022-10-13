using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Entities.OrderAggregate;
using Fnunez.Ena.Core.Interfaces;
using Fnunez.Ena.Core.Specifications;

namespace Fnunez.Ena.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;

    public OrderService(
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IPaymentService paymentService)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId,
        string basketId, Address shippingAddress)
    {
        CustomerBasket basket = await _basketRepository.GetBasketAsync(basketId);

        var items = new List<OrderItem>();
        foreach (BasketItem item in basket.Items)
        {
            Product product = await _unitOfWork
                .Repository<Product>().GetByIdAsync(item.Id);

            var itemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, product.Price, item.Quantity);
            items.Add(orderItem);
        }

        DeliveryMethod deliveryMethod = await _unitOfWork
            .Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

        decimal subtotal = items.Sum(item => item.Price * item.Quantity);

        var specification = new OrderByPaymentIntentIdWithItemsSpecification(basket.PaymentIntentId);
        var existingOrder = await _unitOfWork
            .Repository<Order>()
            .GetFirstOrDefaultAsync(specification);

        if (existingOrder != null)
        {
            _unitOfWork.Repository<Order>().Delete(existingOrder);
            await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
        }

        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal, basket.PaymentIntentId);
        _unitOfWork.Repository<Order>().Add(order);

        int result = await _unitOfWork.CompleteAsync();

        if (result <= 0)
            return null;

        await _basketRepository.DeleteBasketAsync(basketId);

        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().GetListAllAsync();
    }

    public async Task<Order> GetOrderAsync(int id, string buyerEmail)
    {
        var specification = new OrdersWithItemsSpecification(id, buyerEmail);

        return await _unitOfWork.Repository<Order>()
            .GetFirstOrDefaultAsync(specification);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersAsync(string buyerEmail)
    {
        var specification = new OrdersWithItemsSpecification(buyerEmail);

        return await _unitOfWork.Repository<Order>()
            .GetListAsync(specification);
    }
}