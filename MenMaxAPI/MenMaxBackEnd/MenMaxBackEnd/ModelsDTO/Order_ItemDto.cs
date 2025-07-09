namespace MenMaxBackEnd.ModelsDTO
{
    public class Order_ItemDto
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public ProductDto Product { get; set; }
    }
}
