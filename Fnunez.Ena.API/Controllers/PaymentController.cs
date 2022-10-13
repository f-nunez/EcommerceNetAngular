using System.Net;
using Fnunez.Ena.API.Errors;
using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Fnunez.Ena.API.Controllers;

public class PaymentController : BaseApiController
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;
    private readonly string _webhookSecret;

    public PaymentController(
        IPaymentService paymentService,
        ILogger<PaymentController> logger,
        IConfiguration configuration)
    {
        _paymentService = paymentService;
        _logger = logger;
        _webhookSecret = configuration["Stripe:WebhookSecretKet"];
    }

    [Authorize]
    [HttpPost("{basketid}")]
    public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
    {
        CustomerBasket basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

        if (basket != null)
            return basket;

        return BadRequest(
            new ApiResponse(
                (int)HttpStatusCode.BadRequest,
                "There is a problem with your basket"
            )
        );
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
        string json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _webhookSecret);

        PaymentIntent paymentIntent;
        Core.Entities.OrderAggregate.Order order;

        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                _logger.LogInformation("Payment Succeeded");
                order = await _paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                _logger.LogInformation("Order updated to payment received: ", order.Id);
                break;
            case "payment_intent.payment_failed":
                paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                _logger.LogInformation("Payment failed: ", paymentIntent.Id);
                order = await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                _logger.LogInformation("Payment failed: ", order.Id);
                break;
        }

        return new EmptyResult();
    }
}