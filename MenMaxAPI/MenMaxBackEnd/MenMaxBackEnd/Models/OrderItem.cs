using System;
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
