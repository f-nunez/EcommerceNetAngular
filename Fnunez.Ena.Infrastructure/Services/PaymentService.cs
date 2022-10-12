using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Entities.OrderAggregate;
using Fnunez.Ena.Core.Interfaces;
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
        decimal shippingPrice = 0m;

        if (basket.DeliveryMethodId.HasValue)
        {
            DeliveryMethod deliveryMethod = await _unitOfWork
                .Repository<DeliveryMethod>()
                .GetByIdAsync((int)basket.DeliveryMethodId);

            shippingPrice = deliveryMethod.Price;
        }

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
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };

            paymentIntent = await paymentIntentService.CreateAsync(options);
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
            };

            await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
        }

        await _basketRepository.UpdateBasketAsync(basket);

        return basket;
    }
}