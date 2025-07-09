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


