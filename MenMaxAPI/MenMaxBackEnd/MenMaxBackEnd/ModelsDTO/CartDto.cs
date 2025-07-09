namespace MenMaxBackEnd.ModelsDTO
{
    public class CartDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
        public List<ProductDto> Product { get; set; }
    }
}