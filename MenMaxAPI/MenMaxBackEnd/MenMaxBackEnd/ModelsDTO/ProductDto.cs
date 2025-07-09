namespace MenMaxBackEnd.ModelsDTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int? Sold { get; set; }
        public int? IsActive { get; set; }
        public int? IsSelling { get; set; }
        public DateOnly? CreatedAt { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public List<ProductImageDto> ProductImage { get; set; }
        public List<CartDto> CartDto { get; set; }
    }
}
