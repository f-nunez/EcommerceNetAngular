using Fnunez.Ena.Core.Entities.OrderAggregate;

namespace Fnunez.Ena.Core.Specifications;

public class OrdersWithItemsSpecification : BaseSpecifcation<Order>
{
    public OrdersWithItemsSpecification(string buyerEmail)
        : base(o => o.BuyerEmail == buyerEmail)
    {
        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);
        AddOrderByDescending(o => o.OrderDate);
    }

    public OrdersWithItemsSpecification(int id, string buyerEmail)
        : base(o => o.Id == id && o.BuyerEmail == buyerEmail)
    {
        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);
    }
}