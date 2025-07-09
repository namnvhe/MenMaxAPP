using System;
using System.Collections.Generic;

namespace MenMaxBackEnd.Models;

public partial class Order
{
    public int Id { get; set; }

    public string? Address { get; set; }

    public DateOnly? BookingDate { get; set; }

    public string? Country { get; set; }

    public string? Email { get; set; }

    public string? Fullname { get; set; }

    public string? Note { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Phone { get; set; }

    public string? Status { get; set; }

    public int? Total { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User? User { get; set; }
}
