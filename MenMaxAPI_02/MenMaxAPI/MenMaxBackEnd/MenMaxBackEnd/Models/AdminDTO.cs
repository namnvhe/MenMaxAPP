namespace MenMaxBackEnd.Models
{
    public class AdminDTO
    {
        // DTOs/AdminDtos.cs
        public class AdminSignInRequestDto
        {
            public string LoginName { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class AdminSignInResponseDto
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public UserDto? Admin { get; set; }
        }

        public class DashboardDto
        {
            public int TotalOrder { get; set; }
            public int TotalProduct { get; set; }
            public int TotalUser { get; set; }
            public int TotalCategory { get; set; }
        }

        public class InvoiceDto
        {
            public OrderDto? Order { get; set; }
            public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        }

        public class OrderPageDto
        {
            public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
            public int TotalCount { get; set; }
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int TotalPages { get; set; }
        }

        public class SendMessageRequestDto
        {
            public string Message { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }

        public class WalletDto
        {
            public int TotalZaloPay { get; set; }
            public int TotalDelivery { get; set; }
            public int TotalOrder { get; set; }
        }
    }


}
