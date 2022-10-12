using System.Net;
using Fnunez.Ena.API.Errors;
using Fnunez.Ena.Core.Entities;
using Fnunez.Ena.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fnunez.Ena.API.Controllers;

public class PaymentController : BaseApiController
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
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
}