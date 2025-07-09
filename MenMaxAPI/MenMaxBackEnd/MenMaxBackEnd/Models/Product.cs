using System;
using System.Collections.Generic;

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
