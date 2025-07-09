using System;
using System.Collections.Generic;

namespace MenMaxBackEnd.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int Count { get; set; }

    public int? ProductId { get; set; }

    public string? UserId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}

public class CartDto
{
    public int Id { get; set; }
    public int Count { get; set; }
    public int? ProductId { get; set; }
    public string? UserId { get; set; }
    public string? ProductName { get; set; }
    public int? ProductPrice { get; set; }
    public string? ProductImage { get; set; }
    public string? UserName { get; set; }
}