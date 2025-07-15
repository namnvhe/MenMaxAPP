using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MenMaxBackEnd.Models;

public partial class Product
{
    public int Id { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public string? Description { get; set; }

    public int? IsActive { get; set; }

    public int? IsSelling { get; set; }

    public int? Price { get; set; }

    public string? ProductName { get; set; }

    public int? Quantity { get; set; }

    public int? Sold { get; set; }

    public int? CategoryId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}

public class ProductDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("product_Name")]
    public string? ProductName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("sold")]
    public int Sold { get; set; }

    [JsonPropertyName("is_Active")]
    public int IsActive { get; set; }

    [JsonPropertyName("is_Selling")]
    public int IsSelling { get; set; }
    [JsonPropertyName("created_At")]
    public DateOnly? CreatedAt { get; set; }
    [JsonPropertyName("price")]
    public int Price { get; set; }
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    // public CategoryDto Category { get; set; }

    [JsonPropertyName("productImage")]
    public List<ProductImageDto> ProductImages { get; set; }
    //[JsonPropertyName("order_Item")]
    //public List<OrderItemDto> OrderItems { get; set; }

    [JsonPropertyName("cartDto")]
    public List<CartDto> CartDto { get; set; }
}
