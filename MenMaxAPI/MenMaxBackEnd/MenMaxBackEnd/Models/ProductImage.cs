using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MenMaxBackEnd.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public string? UrlImage { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}

public class ProductImageDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("url_Image")]
    public string UrlImage { get; set; }
    // public ProductDto Product { get; set; }
}