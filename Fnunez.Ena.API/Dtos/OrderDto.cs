namespace Fnunez.Ena.API.Dtos;

public class OrderDto
{
    public string BasketId { get; set; }
    public int DeliveryMethodId { get; set; }
    public AddressDto ShippingToAddress { get; set; }
}