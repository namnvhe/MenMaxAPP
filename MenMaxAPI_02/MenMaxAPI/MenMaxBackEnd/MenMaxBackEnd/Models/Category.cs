using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MenMaxBackEnd.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? CategoryName { get; set; }

    public string? CategoryImage { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
public class CategoryDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("category_Name")]
    public string CategoryName { get; set; }
    [JsonPropertyName("category_Image")]
    public string CategoryImage { get; set; }
    [JsonPropertyName("product")]
    public List<ProductDto> Products { get; set; }
}
