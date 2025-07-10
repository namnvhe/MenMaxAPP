using System;
using System.Collections.Generic;

namespace MenMaxBackEnd.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string? Avatar { get; set; }

    public string? Email { get; set; }

    public string? LoginType { get; set; }

    public string? Password { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Role { get; set; }

    public string? UserName { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
public class UserDto
{
    public string Id { get; set; }
    public string LoginType { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public string Avatar { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    // public List<OrderDto> Orders { get; set; }
    // public List<CartDto> Carts { get; set; }
}