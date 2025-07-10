using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MenMaxBackEnd.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int? Count { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    [JsonIgnore]
    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
public class OrderItemDto
{
    public int Id { get; set; }
    public int? Count { get; set; }
    public int? OrderId { get; set; }
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public int? ProductPrice { get; set; }
    public string? ProductImage { get; set; }
}