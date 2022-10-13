using Fnunez.Ena.Core.Entities.OrderAggregate;

namespace Fnunez.Ena.Core.Specifications;

public class OrderByPaymentIntentWithItemsSpecification : BaseSpecifcation<Order>
{
    public OrderByPaymentIntentWithItemsSpecification(string paymentIntentId)
        : base(o => o.PaymentIntentId == paymentIntentId)
    {
    }
}