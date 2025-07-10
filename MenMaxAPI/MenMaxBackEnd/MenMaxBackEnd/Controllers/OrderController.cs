using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenMaxBackEnd.Models;
using MenMaxBackEnd.Services;
using System.ComponentModel.DataAnnotations;

namespace MenMaxBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly MenMaxContext _context;
        private readonly ModelMapper _modelMapper;

        public OrderController(
            MenMaxContext context,
            ModelMapper modelMapper)
        {
            _context = context;
            _modelMapper = modelMapper;
        }

        [HttpPost("placeorder")]
        public ActionResult<OrderDto> PlaceOrder(
            [FromForm] string user_id,
            [FromForm] string fullname,
            [FromForm] string phoneNumber,
            [FromForm] string address,
            [FromForm] string paymentMethod)
        {
            try
            {
                // ✅ Lấy danh sách cart của user trực tiếp từ context
                var listCart = _context.Carts
                    .Include(c => c.Product)
                    .Include(c => c.User)
                    .Where(c => c.UserId == user_id)
                    .ToList();

                if (listCart == null || !listCart.Any())
                {
                    return BadRequest("Cart is empty");
                }

                // ✅ Tìm user trực tiếp từ context
                var user = _context.Users
                    .FirstOrDefault(u => u.Id == user_id && u.Role == "user");

                if (user == null)
                {
                    return BadRequest("User not found");
                }

                // ✅ Tạo order mới
                var newOrder = new Order
                {
                    UserId = user_id,
                    Fullname = fullname,
                    BookingDate = DateOnly.FromDateTime(DateTime.Now),
                    Country = "Việt Nam",
                    Email = user.Email,
                    PaymentMethod = paymentMethod,
                    Address = address,
                    Note = null,
                    Phone = phoneNumber,
                    Status = "Pending",
                    Total = 0
                };

                // ✅ Tính tổng tiền
                int total = 0;
                foreach (var cart in listCart)
                {
                    total += (cart.Product.Price ?? 0) * cart.Count;
                }
                newOrder.Total = total;

                // ✅ Lưu order trực tiếp vào context
                _context.Orders.Add(newOrder);
                _context.SaveChanges();

                // ✅ Xử lý từng item trong cart
                foreach (var cart in listCart)
                {
                    // Kiểm tra số lượng tồn kho
                    if (cart.Count > cart.Product.Quantity)
                    {
                        // Xóa order nếu không đủ hàng
                        _context.Orders.Remove(newOrder);
                        _context.SaveChanges();
                        return BadRequest($"Insufficient stock for product: {cart.Product.ProductName}");
                    }

                    // Cập nhật số lượng và sold của product
                    cart.Product.Quantity -= cart.Count;
                    cart.Product.Sold += cart.Count;
                    _context.Products.Update(cart.Product);

                    // Tạo order item
                    var newOrderItem = new OrderItem
                    {
                        Count = cart.Count,
                        OrderId = newOrder.Id,
                        ProductId = cart.Product.Id
                    };
                    _context.OrderItems.Add(newOrderItem);

                    // Xóa cart item
                    _context.Carts.Remove(cart);
                }

                // ✅ Lưu tất cả thay đổi
                _context.SaveChanges();

                // ✅ Lấy order đã hoàn thành với đầy đủ thông tin
                var completedOrder = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .FirstOrDefault(o => o.Id == newOrder.Id);

                var orderDto = _modelMapper.Map<Order, OrderDto>(completedOrder);
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PlaceOrder: {ex.Message}");
                return StatusCode(500, "An error occurred while placing the order");
            }
        }

        [HttpGet("order")]
        public ActionResult<List<OrderDto>> GetOrder([FromQuery] string user_id)
        {
            try
            {
                Console.WriteLine($"Getting orders for user: {user_id}");

                if (string.IsNullOrEmpty(user_id))
                {
                    return BadRequest("User ID is required");
                }

                // ✅ Lấy danh sách order trực tiếp từ context
                var listOrder = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .Where(o => o.UserId == user_id)
                    .OrderByDescending(o => o.BookingDate)
                    .ToList();

                var listOrderDto = new List<OrderDto>();
                foreach (var order in listOrder)
                {
                    var orderDto = _modelMapper.Map<Order, OrderDto>(order);
                    Console.WriteLine($"Order ID: {orderDto.Id}");
                    listOrderDto.Add(orderDto);
                }

                return Ok(listOrderDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOrder: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving orders");
            }
        }

        [HttpGet("ordermethod")]
        public ActionResult<List<OrderDto>> GetOrderByPaymentMethod(
            [FromQuery] string user_id,
            [FromQuery] string method)
        {
            try
            {
                Console.WriteLine($"Getting orders for user: {user_id}, method: {method}");

                if (string.IsNullOrEmpty(user_id) || string.IsNullOrEmpty(method))
                {
                    return BadRequest("User ID and payment method are required");
                }

                // ✅ Lấy order theo payment method trực tiếp từ context
                var listOrder = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .Where(o => o.UserId == user_id && o.PaymentMethod == method)
                    .OrderByDescending(o => o.BookingDate)
                    .ToList();

                var listOrderDto = new List<OrderDto>();
                foreach (var order in listOrder)
                {
                    var orderDto = _modelMapper.Map<Order, OrderDto>(order);
                    listOrderDto.Add(orderDto);
                }

                foreach (var order in listOrder)
                {
                    Console.WriteLine($"Order ID: {order.Id}");
                }

                return Ok(listOrderDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOrderByPaymentMethod: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving orders by payment method");
            }
        }

        [HttpGet("orderhistory")]
        public ActionResult<List<OrderDto>> GetOrderHistory([FromQuery] string user_id)
        {
            try
            {
                if (string.IsNullOrEmpty(user_id))
                {
                    return BadRequest("User ID is required");
                }

                // ✅ Logic này đã đúng, giữ nguyên
                var orders = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .Where(o => o.UserId == user_id)
                    .OrderByDescending(o => o.BookingDate)
                    .ToList();

                var orderDtos = _modelMapper.MapList<Order, OrderDto>(orders);
                return Ok(orderDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOrderHistory: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving order history");
            }
        }

        [HttpPut("updatestatus/{orderId}")]
        public ActionResult<OrderDto> UpdateOrderStatus(int orderId, [FromForm] string status)
        {
            try
            {
                // ✅ Tìm order trực tiếp từ context
                var order = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                // ✅ Cập nhật status và lưu trực tiếp
                order.Status = status;
                _context.Orders.Update(order);
                _context.SaveChanges();

                var orderDto = _modelMapper.Map<Order, OrderDto>(order);
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateOrderStatus: {ex.Message}");
                return StatusCode(500, "An error occurred while updating order status");
            }
        }

        [HttpDelete("cancel/{orderId}")]
        public ActionResult<string> CancelOrder(int orderId, [FromQuery] string user_id)
        {
            try
            {
                // ✅ Logic này đã đúng, giữ nguyên
                var order = _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .FirstOrDefault(o => o.Id == orderId && o.UserId == user_id);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                if (order.Status != "Pending")
                {
                    return BadRequest("Cannot cancel order that is not pending");
                }

                // ✅ Hoàn trả số lượng sản phẩm trực tiếp
                foreach (var orderItem in order.OrderItems)
                {
                    orderItem.Product.Quantity += orderItem.Count;
                    orderItem.Product.Sold -= orderItem.Count;
                    _context.Products.Update(orderItem.Product);
                }

                // ✅ Cập nhật trạng thái order trực tiếp
                order.Status = "Cancelled";
                _context.Orders.Update(order);
                _context.SaveChanges();

                return Ok("Order cancelled successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CancelOrder: {ex.Message}");
                return StatusCode(500, "An error occurred while cancelling the order");
            }
        }

        [HttpGet("orderstatus")]
        public ActionResult<List<OrderDto>> GetOrdersByStatus(
            [FromQuery] string user_id,
            [FromQuery] string status)
        {
            try
            {
                if (string.IsNullOrEmpty(user_id) || string.IsNullOrEmpty(status))
                {
                    return BadRequest("User ID and status are required");
                }

                // ✅ Lấy order theo status trực tiếp từ context
                var orders = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .Where(o => o.UserId == user_id && o.Status == status)
                    .OrderByDescending(o => o.BookingDate)
                    .ToList();

                var orderDtos = _modelMapper.MapList<Order, OrderDto>(orders);
                return Ok(orderDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOrdersByStatus: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving orders by status");
            }
        }

        [HttpGet("orderdetail/{orderId}")]
        public ActionResult<OrderDto> GetOrderDetail(int orderId, [FromQuery] string user_id)
        {
            try
            {
                if (string.IsNullOrEmpty(user_id))
                {
                    return BadRequest("User ID is required");
                }

                // ✅ Lấy chi tiết order trực tiếp từ context
                var order = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .FirstOrDefault(o => o.Id == orderId && o.UserId == user_id);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                var orderDto = _modelMapper.Map<Order, OrderDto>(order);
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetOrderDetail: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving order detail");
            }
        }
    }
}
