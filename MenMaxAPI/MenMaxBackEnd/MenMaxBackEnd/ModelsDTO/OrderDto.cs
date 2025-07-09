namespace MenMaxBackEnd.ModelsDTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public DateOnly BookingDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string Fullname { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public List<Order_ItemDto> OrderItem { get; set; }
        public UserDto User { get; set; }
    }
}
