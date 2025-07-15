using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Net.Mail;
using MenMaxBackEnd.Models;
using MenMaxBackEnd.Services;
using static MenMaxBackEnd.Models.AdminDTO;

namespace MenMaxBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly MenMaxContext _context;
        private readonly ModelMapper _modelMapper;
        private readonly IConfiguration _configuration;

        public AdminController(MenMaxContext context, ModelMapper modelMapper, IConfiguration configuration)
        {
            _context = context;
            _modelMapper = modelMapper;
            _configuration = configuration;
        }

        [HttpPost("signin")]
        public ActionResult<AdminSignInRequestDto> SignInAdmin([FromBody] AdminSignInRequestDto request)
        {
            try
            {
                // Tìm admin user
                var admin = _context.Users.FirstOrDefault(u => u.Id == request.LoginName && u.Role == "admin");
                Console.WriteLine(admin);

                if (admin == null)
                {
                    return BadRequest(new { Message = "Username or Password is not correct!" });
                }

                string decodedValue = Encoding.UTF8.GetString(Convert.FromBase64String(admin.Password));
                if (!decodedValue.Equals(request.Password))
                {
                    return BadRequest(new { Message = "Username or Password is not correct!" });
                }

                // Lưu admin info vào session
                HttpContext.Session.SetString("admin", System.Text.Json.JsonSerializer.Serialize(admin));
                Console.WriteLine(admin);

                var response = new AdminSignInResponseDto
                {
                    Success = true,
                    Message = "Login successful",
                    Admin = _modelMapper.Map<UserDto>(admin)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("logout")]
        public ActionResult<object> LogoutAdmin()
        {
            try
            {
                HttpContext.Session.Remove("admin");
                return Ok(new { Success = true, Message = "Logout successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("dashboard")]
        public ActionResult<DashboardDto> Dashboard()
        {
            try
            {
                // Kiểm tra admin session
                var adminSession = HttpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(adminSession))
                {
                    return Unauthorized(new { Message = "Admin not authenticated" });
                }

                Console.WriteLine("======");

                // Lấy thống kê
                var totalOrders = _context.Orders.Count();
                var totalProducts = _context.Products.Count();
                var totalUsers = _context.Users.Where(u => u.Role == "user").Count();
                var totalCategories = _context.Categories.Count();

                var dashboard = new DashboardDto
                {
                    TotalOrder = totalOrders,
                    TotalProduct = totalProducts,
                    TotalUser = totalUsers,
                    TotalCategory = totalCategories
                };

                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("invoice/{id}")]
        public ActionResult<InvoiceDto> Invoice(int id)
        {
            try
            {
                // Kiểm tra admin session
                var adminSession = HttpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(adminSession))
                {
                    return Unauthorized(new { Message = "Admin not authenticated" });
                }

                var order = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .FirstOrDefault(o => o.Id == id);

                if (order == null)
                {
                    return NotFound(new { Message = "Order not found" });
                }

                var invoice = new InvoiceDto
                {
                    Order = _modelMapper.Map<OrderDto>(order),
                    OrderItems = _modelMapper.MapList<OrderItem, OrderItemDto>(order.OrderItems.ToList())
                };

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("orders")]
        public ActionResult<OrderPageDto> DashboardOrders([FromQuery] int page = 0, [FromQuery] int pageSize = 3)
        {
            try
            {
                // Kiểm tra admin session
                var adminSession = HttpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(adminSession))
                {
                    return Unauthorized(new { Message = "Admin not authenticated" });
                }

                var totalOrders = _context.Orders.Count();
                var orders = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .OrderByDescending(o => o.BookingDate)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                var orderDtos = _modelMapper.MapList<Order, OrderDto>(orders);

                var result = new OrderPageDto
                {
                    Orders = orderDtos,
                    TotalCount = totalOrders,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalOrders / pageSize)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("send-message")]
        public ActionResult<object> SendMessage([FromBody] SendMessageRequestDto request)
        {
            try
            {
                Console.WriteLine(request.Message);
                Console.WriteLine(request.Email);

                // Gửi email trực tiếp
                var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"])
                {
                    Port = int.Parse(_configuration["Email:Port"]),
                    Credentials = new System.Net.NetworkCredential(
                        _configuration["Email:Username"],
                        _configuration["Email:Password"]
                    ),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("haovo1512@gmail.com"),
                    Subject = "This is message from Male fashion.",
                    Body = request.Message,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(request.Email);

                smtpClient.Send(mailMessage);

                return Ok(new { Success = true, Message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }

        [HttpDelete("orders/{id}")]
        public ActionResult<object> DeleteOrder(int id)
        {
            try
            {
                // Kiểm tra admin session
                var adminSession = HttpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(adminSession))
                {
                    return Unauthorized(new { Message = "Admin not authenticated" });
                }

                var order = _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefault(o => o.Id == id);

                Console.WriteLine(order);

                if (order != null)
                {
                    // Xóa order items trước
                    foreach (var orderItem in order.OrderItems)
                    {
                        _context.OrderItems.Remove(orderItem);
                    }

                    // Xóa order
                    _context.Orders.Remove(order);
                    _context.SaveChanges();
                }

                return Ok(new { Success = true, Message = "Order deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("wallet")]
        public ActionResult<WalletDto> DashboardWallet()
        {
            try
            {
                // Kiểm tra admin session
                var adminSession = HttpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(adminSession))
                {
                    return Unauthorized(new { Message = "Admin not authenticated" });
                }

                var allOrders = _context.Orders.ToList();
                var paymentWithZaloPay = _context.Orders
                    .Where(o => o.PaymentMethod == "Pay with ZaloPay")
                    .ToList();
                var paymentOnDelivery = _context.Orders
                    .Where(o => o.PaymentMethod == "Pay on Delivery")
                    .ToList();

                int totalZaloPay = 0;
                int totalDelivery = 0;

                foreach (var order in paymentWithZaloPay)
                {
                    totalZaloPay += order.Total ?? 0;
                }

                foreach (var order in paymentOnDelivery)
                {
                    totalDelivery += order.Total ?? 0;
                }

                var wallet = new WalletDto
                {
                    TotalZaloPay = totalZaloPay,
                    TotalDelivery = totalDelivery,
                    TotalOrder = allOrders.Count
                };

                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Các API bổ sung hữu ích
        [HttpGet("users")]
        public ActionResult<List<UserDto>> GetAllUsers([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            try
            {
                var adminSession = HttpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(adminSession))
                {
                    return Unauthorized(new { Message = "Admin not authenticated" });
                }

                var users = _context.Users
                    .Where(u => u.Role == "user")
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                var userDtos = _modelMapper.MapList<User, UserDto>(users);
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("products")]
        public ActionResult<List<ProductDto>> GetAllProducts([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
        {
            try
            {
                var adminSession = HttpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(adminSession))
                {
                    return Unauthorized(new { Message = "Admin not authenticated" });
                }

                var products = _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                var productDtos = _modelMapper.MapList<Product, ProductDto>(products);
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("stats")]
        public ActionResult<object> GetDetailedStats()
        {
            try
            {
                var adminSession = HttpContext.Session.GetString("admin");
                if (string.IsNullOrEmpty(adminSession))
                {
                    return Unauthorized(new { Message = "Admin not authenticated" });
                }

                var stats = new
                {
                    TotalRevenue = _context.Orders.Sum(o => o.Total ?? 0),
                    TotalOrdersToday = _context.Orders.Count(o => o.BookingDate == DateOnly.FromDateTime(DateTime.Today)),
                    TotalProductsSold = _context.Products.Sum(p => p.Sold ?? 0),
                    AvgOrderValue = _context.Orders.Average(o => o.Total ?? 0),
                    TopSellingProducts = _context.Products
                        .OrderByDescending(p => p.Sold)
                        .Take(5)
                        .Select(p => new { p.ProductName, p.Sold })
                        .ToList()
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
