using Fnunez.Ena.Core.Entities.OrderAggregate;

namespace Fnunez.Ena.Core.Specifications;

public class OrderByPaymentIntentIdWithItemsSpecification : BaseSpecifcation<Order>
{
    public OrderByPaymentIntentIdWithItemsSpecification(string paymentIntentId)
        : base(o => o.PaymentIntentId == paymentIntentId)
    {
    }
}