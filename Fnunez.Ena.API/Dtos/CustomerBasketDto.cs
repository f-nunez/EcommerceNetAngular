using System.ComponentModel.DataAnnotations;

namespace Fnunez.Ena.API.Dtos;

public class CustomerBasketDto
{
    [Required]
    public string Id { get; set; }
    public List<BasketItemDto> Items { get; set; }
}