namespace Fnunez.Ena.Core.Entities;

public class CustomerBasket
{
    public string Id { get; set; }
    public List<BasketItem> Items { get; set; } = new List<BasketItem>();

    public CustomerBasket()
    {
    }

    public CustomerBasket(string id)
    {
        Id = id;
    }
}