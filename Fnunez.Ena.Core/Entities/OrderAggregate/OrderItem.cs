namespace Fnunez.Ena.Core.Entities.OrderAggregate;

public class OrderItem : BaseEntity
{
    public ProductItemOrdered ProductItemOrdered { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public OrderItem()
    {
    }

    public OrderItem(ProductItemOrdered productItemOrdered, decimal price, int quantity)
    {
        ProductItemOrdered = productItemOrdered;
        Price = price;
        Quantity = quantity;
    }
}