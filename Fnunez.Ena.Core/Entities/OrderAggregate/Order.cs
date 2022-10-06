using System;
using System.ComponentModel.DataAnnotations;

namespace Fnunez.Ena.Core.Entities.OrderAggregate;

public class Order : BaseEntity
{
    public string BuyerEmail { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
    [Required]
    public Address ShipToAddress { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; }
    public decimal Subtotal { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string PaymentIntentId { get; set; }

    public Order()
    {
    }

    public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, Address shipToAddress,
        DeliveryMethod deliveryMethod, decimal subtotal)
    {
        OrderItems = orderItems;
        BuyerEmail = buyerEmail;
        ShipToAddress = shipToAddress;
        DeliveryMethod = deliveryMethod;
        Subtotal = subtotal;
    }

    public decimal GetTotal()
    {
        return Subtotal + DeliveryMethod.Price;
    }
}