using MenMaxBackEnd.Models;
using MenMaxBackEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly MenMaxContext _context;
        private readonly ModelMapper _modelMapper;

        public OrderController(MenMaxContext context, ModelMapper modelMapper)
        {
            _context = context;
            _modelMapper = modelMapper;
        }
        [HttpPost("placeorder")]
        public ActionResult<OrderDto> PlaceOrder([FromForm] string user_id, [FromForm] string fullname, [FromForm] string phoneNumber, [FromForm] string address, [FromForm] string paymentMethod)
        {
            var listCart = _context.Carts
                .Include(c => c.Product)
                .Include(c => c.User)
                .Where(c => c.UserId == user_id)
                .ToList();

            var user = _context.Users.FirstOrDefault(u => u.Id == user_id && u.Role == "user");
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var newOrder = new Order
            {
                User = user,
                Fullname = fullname,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                Country = "Việt Nam",
                Email = user.Email,
                PaymentMethod = paymentMethod,
                Address = address,
                Note = null,
                Phone = phoneNumber,
                Status = "Pending",
                Total = listCart.Sum(y => y.Product.Price * y.Count)
            };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            foreach (var cart in listCart)
            {
                if (cart.Count > cart.Product.Quantity)
                {
                    _context.Orders.Remove(newOrder);
                    _context.SaveChanges();
                    return Ok(null);
                }

                cart.Product.Quantity -= cart.Count;
                cart.Product.Sold += cart.Count;
                _context.Products.Update(cart.Product);

                var newOrderItem = new OrderItem
                {
                    Count = cart.Count,
                    OrderId = newOrder.Id,
                    ProductId = cart.Product.Id
                };

                _context.OrderItems.Add(newOrderItem);
                _context.Carts.Remove(cart);
            }

            _context.SaveChanges();

            var savedOrder = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems) // Include OrderItems for mapping
                .FirstOrDefault(o => o.Id == newOrder.Id);

            var savedOrderDto = _modelMapper.Map<Order, OrderDto>(savedOrder);
            return Ok(savedOrderDto);
        }

        [HttpGet("order")]
        public ActionResult<List<OrderDto>> GetOrder([FromQuery] string user_id)
        {
            Console.WriteLine(user_id);

            var listOrder = _context.Orders
                .Include(o => o.User)
                .Where(o => o.UserId == user_id)
                .ToList();

            var listOrderDto = _modelMapper.Map<List<Order>, List<OrderDto>>(listOrder);

            foreach (var orderDto in listOrderDto)
            {
                Console.WriteLine(orderDto.Id);
            }

            return Ok(listOrderDto);
        }

        [HttpGet("ordermethod")]
        public ActionResult<List<OrderDto>> GetOrderByPaymentMethod([FromQuery] string user_id, [FromQuery] string payment_method)
        {
            Console.WriteLine(user_id);

            var listOrder = _context.Orders
                .Include(o => o.User)
                .Where(o => o.UserId == user_id && o.PaymentMethod == payment_method)
                .ToList();

            var listOrderDto = _modelMapper.Map<List<Order>, List<OrderDto>>(listOrder);

            foreach (var order in listOrder)
            {
                Console.WriteLine(order.Id);
            }

            return Ok(listOrderDto);
        }
    }
}