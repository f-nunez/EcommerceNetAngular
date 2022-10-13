using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Entities.OrderAggregate;
using Fnunez.Ena.Core.Interfaces;
using Fnunez.Ena.Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Fnunez.Ena.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(
        IBasketRepository basketRepository,
        IConfiguration configuration,
        IUnitOfWork unitOfWork)
    {
        _basketRepository = basketRepository;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
    {
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

        CustomerBasket basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket == null)
            return null;

        DeliveryMethod deliveryMethod = null;

        if (basket.DeliveryMethodId.HasValue)
            deliveryMethod = await _unitOfWork
                .Repository<DeliveryMethod>()
                .GetByIdAsync((int)basket.DeliveryMethodId);

        foreach (var item in basket.Items)
        {
            Core.Entities.Product productItem = await _unitOfWork
                .Repository<Core.Entities.Product>()
                .GetByIdAsync(item.Id);

            if (productItem.Price != item.Price)
                item.Price = productItem.Price;
        }

        var paymentIntentService = new PaymentIntentService();

        PaymentIntent paymentIntent;

        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var options = MapPaymentIntentCreateOptions(basket, deliveryMethod);
            paymentIntent = await paymentIntentService.CreateAsync(options);
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;
        }
        else
        {
            var options = MapPaymentIntentUpdateOptions(basket, deliveryMethod);
            await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
        }

        await _basketRepository.UpdateBasketAsync(basket);

        return basket;
    }

    public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
    {
        var specification = new OrderByPaymentIntentWithItemsSpecification(paymentIntentId);
        Order order = await _unitOfWork
            .Repository<Order>()
            .GetFirstOrDefaultAsync(specification);

        if (order == null)
            return null;

        order.Status = OrderStatus.PaymentFailed;

        _unitOfWork.Repository<Order>().Update(order);

        await _unitOfWork.CompleteAsync();

        return order;
    }

    public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
    {
        var specification = new OrderByPaymentIntentWithItemsSpecification(paymentIntentId);
        Order order = await _unitOfWork
            .Repository<Order>()
            .GetFirstOrDefaultAsync(specification);

        if (order == null)
            return null;

        order.Status = OrderStatus.PaymentReceived;

        _unitOfWork.Repository<Order>().Update(order);

        await _unitOfWork.CompleteAsync();

        return order;
    }

    private PaymentIntentCreateOptions MapPaymentIntentCreateOptions(CustomerBasket basket,
        DeliveryMethod deliveryMethod)
    {
        return new PaymentIntentCreateOptions
        {
            Amount = CalculateTotalAmountToPay(basket, deliveryMethod),
            Currency = "usd",
            PaymentMethodTypes = new List<string> { "card" }
        };
    }

    private PaymentIntentUpdateOptions MapPaymentIntentUpdateOptions(CustomerBasket basket,
        DeliveryMethod deliveryMethod)
    {
        return new PaymentIntentUpdateOptions
        {
            Amount = CalculateTotalAmountToPay(basket, deliveryMethod),
        };
    }

    private long CalculateTotalAmountToPay(CustomerBasket basket, DeliveryMethod deliveryMethod)
    {
        decimal totalAmountByItems = basket.Items.Sum(i => i.Quantity * i.Price) * 100;
        decimal totalAmountByShipping = deliveryMethod != null ? deliveryMethod.Price * 100 : 0;

        return Convert.ToInt64(totalAmountByItems + totalAmountByShipping);
    }
}