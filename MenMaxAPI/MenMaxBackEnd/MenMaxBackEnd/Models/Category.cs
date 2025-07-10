using System;
using System.Collections.Generic;

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
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public string CategoryImage { get; set; }
    public List<ProductDto> Products { get; set; }
}
