using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MenMaxBackEnd.Models;

public partial class User
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("login_Type")]
    public string? LoginType { get; set; }
    [JsonPropertyName("role")]
    public string? Role { get; set; }
    [JsonPropertyName("password")]
    public string? Password { get; set; }
    [JsonPropertyName("user_Name")]
    public string? UserName { get; set; }
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("phone_Number")]
    public string? PhoneNumber { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    [JsonPropertyName("order")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
public class UserDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("login_Type")]
    public string? LoginType { get; set; }
    [JsonPropertyName("role")]
    public string? Role { get; set; }
    [JsonPropertyName("password")]
    public string? Password { get; set; }
    [JsonPropertyName("user_Name")]
    public string? UserName { get; set; }
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("phone_Number")]
    public string? PhoneNumber { get; set; }
    [JsonPropertyName("address")]
    public string? address { get; set; }
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    [JsonPropertyName("order")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}