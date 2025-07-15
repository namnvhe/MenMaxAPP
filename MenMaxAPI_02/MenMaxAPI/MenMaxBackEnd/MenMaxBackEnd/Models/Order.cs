using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
public class OrderDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("booking_Date")]
    public DateOnly? BookingDate { get; set; } // Changed to string to match JSON format "YYYY-MM-DD"

    [JsonPropertyName("payment_Method")]
    public string PaymentMethod { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("fullname")]
    public string Fullname { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("note")]
    public string Note { get; set; }

    [JsonPropertyName("order_Item")]
    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

    [JsonIgnore] // Ignore User to prevent serialization cycle
    public UserDto User { get; set; }
}
