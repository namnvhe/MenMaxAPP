﻿using System;
using System.Collections.Generic;

namespace MenMaxBackEnd.Models;

public partial class OrderItem
{
    public int Id { get; set; }

    public int? Count { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
public class OrderItemDto
{
    public int Id { get; set; }
    public int Count { get; set; }
    public ProductDto Product { get; set; }
    // public OrderDto Order { get; set; }
}
