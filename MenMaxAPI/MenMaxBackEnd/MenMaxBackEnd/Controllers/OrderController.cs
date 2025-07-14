
using MenMaxBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MenMaxBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly MenMaxContext _context;

        public OrderController(MenMaxContext context)
        {
            _context = context;
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
                var listCart = _context.Carts
                    .Include(c => c.Product)
                        .ThenInclude(p => p.ProductImages)
                    .Include(c => c.User)
                    .Where(c => c.UserId == user_id)
                    .ToList();

                if (listCart == null || !listCart.Any())
                {
                    return BadRequest("Cart is empty");
                }

                var user = _context.Users
                    .FirstOrDefault(u => u.Id == user_id && u.Role == "user");

                if (user == null)
                {
                    return BadRequest("User not found");
                }

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

                int total = 0;
                foreach (var cart in listCart)
                {
                    total += (cart.Product.Price ?? 0) * cart.Count;
                }
                newOrder.Total = total;

                _context.Orders.Add(newOrder);
                _context.SaveChanges();

                foreach (var cart in listCart)
                {
                    if (cart.Count > cart.Product.Quantity)
                    {
                        _context.Orders.Remove(newOrder);
                        _context.SaveChanges();
                        return BadRequest($"Insufficient stock for product: {cart.Product.ProductName}");
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

                var completedOrder = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .FirstOrDefault(o => o.Id == newOrder.Id);

                var orderDto = new OrderDto
                {
                    Id = completedOrder.Id,
                    Total = (int)completedOrder.Total,
                    BookingDate = completedOrder.BookingDate,
                    PaymentMethod = completedOrder.PaymentMethod,
                    Status = completedOrder.Status,
                    Fullname = completedOrder.Fullname,
                    Country = completedOrder.Country,
                    Address = completedOrder.Address,
                    Phone = completedOrder.Phone,
                    Email = completedOrder.Email,
                    Note = completedOrder.Note,
                    OrderItems = completedOrder.OrderItems?.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        Count = (int)oi.Count,
                        Product = new ProductDto
                        {
                            Id = oi.Product.Id,
                            ProductName = oi.Product.ProductName,
                            Description = oi.Product.Description,
                            Sold = (int)oi.Product.Sold,
                            IsActive = (int)oi.Product.IsActive,
                            IsSelling = (int)oi.Product.IsSelling,
                            CreatedAt = oi.Product.CreatedAt,
                            Price = oi.Product.Price ?? 0,
                            Quantity = (int)oi.Product.Quantity,
                            ProductImages = oi.Product.ProductImages?.Select(pi => new ProductImageDto
                            {
                                Id = pi.Id,
                                UrlImage = pi.UrlImage
                            }).ToList() ?? new List<ProductImageDto>(),
                            CartDto = null
                        }
                    }).ToList() ?? new List<OrderItemDto>(),
                    User = null // Avoid cycle
                };

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

                var listOrder = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .Where(o => o.UserId == user_id)
                    .OrderByDescending(o => o.BookingDate)
                    .ToList();

                var listOrderDto = listOrder.Select(o => new OrderDto
                {
                    Id = o.Id,
                    Total = (int)o.Total,
                    BookingDate = o.BookingDate,
                    PaymentMethod = o.PaymentMethod,
                    Status = o.Status,
                    Fullname = o.Fullname,
                    Country = o.Country,
                    Address = o.Address,
                    Phone = o.Phone,
                    Email = o.Email,
                    Note = o.Note,
                    OrderItems = o.OrderItems?.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        Count = (int)oi.Count,
                        Product = new ProductDto
                        {
                            Id = oi.Product.Id,
                            ProductName = oi.Product.ProductName,
                            Description = oi.Product.Description,
                            Sold = (int)oi.Product.Sold,
                            IsActive = (int)oi.Product.IsActive,
                            IsSelling = (int)oi.Product.IsSelling,
                            CreatedAt = oi.Product.CreatedAt,
                            Price = oi.Product.Price ?? 0,
                            Quantity = (int)oi.Product.Quantity,
                            ProductImages = oi.Product.ProductImages?.Select(pi => new ProductImageDto
                            {
                                Id = pi.Id,
                                UrlImage = pi.UrlImage
                            }).ToList() ?? new List<ProductImageDto>(),
                            CartDto = null
                        }
                    }).ToList() ?? new List<OrderItemDto>(),
                    User = null // Avoid cycle
                }).ToList();

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

                var listOrder = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .Where(o => o.UserId == user_id && o.PaymentMethod == method)
                    .OrderByDescending(o => o.BookingDate)
                    .ToList();

                var listOrderDto = listOrder.Select(o => new OrderDto
                {
                    Id = o.Id,
                    Total = (int)o.Total,
                    BookingDate = o.BookingDate,
                    PaymentMethod = o.PaymentMethod,
                    Status = o.Status,
                    Fullname = o.Fullname,
                    Country = o.Country,
                    Address = o.Address,
                    Phone = o.Phone,
                    Email = o.Email,
                    Note = o.Note,
                    OrderItems = o.OrderItems?.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        Count = (int)oi.Count,
                        Product = new ProductDto
                        {
                            Id = oi.Product.Id,
                            ProductName = oi.Product.ProductName,
                            Description = oi.Product.Description,
                            Sold = (int)oi.Product.Sold,
                            IsActive = (int)oi.Product.IsActive,
                            IsSelling = (int)oi.Product.IsSelling,
                            CreatedAt = oi.Product.CreatedAt,
                            Price = oi.Product.Price ?? 0,
                            Quantity = (int)oi.Product.Quantity,
                            ProductImages = oi.Product.ProductImages?.Select(pi => new ProductImageDto
                            {
                                Id = pi.Id,
                                UrlImage = pi.UrlImage
                            }).ToList() ?? new List<ProductImageDto>(),
                            CartDto = null
                        }
                    }).ToList() ?? new List<OrderItemDto>(),
                    User = null // Avoid cycle
                }).ToList();

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

                var orders = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .Where(o => o.UserId == user_id)
                    .OrderByDescending(o => o.BookingDate)
                    .ToList();

                var orderDtos = orders.Select(o => new OrderDto
                {
                    Id = o.Id,
                    Total = (int)o.Total,
                    BookingDate = o.BookingDate,
                    PaymentMethod = o.PaymentMethod,
                    Status = o.Status,
                    Fullname = o.Fullname,
                    Country = o.Country,
                    Address = o.Address,
                    Phone = o.Phone,
                    Email = o.Email,
                    Note = o.Note,
                    OrderItems = o.OrderItems?.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        Count = (int)oi.Count,
                        Product = new ProductDto
                        {
                            Id = oi.Product.Id,
                            ProductName = oi.Product.ProductName,
                            Description = oi.Product.Description,
                            Sold = (int)oi.Product.Sold,
                            IsActive = (int)oi.Product.IsActive,
                            IsSelling = (int)oi.Product.IsSelling,
                            CreatedAt = oi.Product.CreatedAt,
                            Price = oi.Product.Price ?? 0,
                            Quantity = (int)oi.Product.Quantity,
                            ProductImages = oi.Product.ProductImages?.Select(pi => new ProductImageDto
                            {
                                Id = pi.Id,
                                UrlImage = pi.UrlImage
                            }).ToList() ?? new List<ProductImageDto>(),
                            CartDto = null
                        }
                    }).ToList() ?? new List<OrderItemDto>(),
                    User = null // Avoid cycle
                }).ToList();

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

                order.Status = status;
                _context.Orders.Update(order);
                _context.SaveChanges();

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    Total = (int)order.Total,
                    BookingDate = order.BookingDate,
                    PaymentMethod = order.PaymentMethod,
                    Status = order.Status,
                    Fullname = order.Fullname,
                    Country = order.Country,
                    Address = order.Address,
                    Phone = order.Phone,
                    Email = order.Email,
                    Note = order.Note,
                    OrderItems = order.OrderItems?.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        Count = (int)oi.Count,
                        Product = new ProductDto
                        {
                            Id = oi.Product.Id,
                            ProductName = oi.Product.ProductName,
                            Description = oi.Product.Description,
                            Sold = (int)oi.Product.Sold,
                            IsActive = (int)oi.Product.IsActive,
                            IsSelling = (int)oi.Product.IsSelling,
                            CreatedAt = oi.Product.CreatedAt,
                            Price = oi.Product.Price ?? 0,
                            Quantity = (int)oi.Product.Quantity,
                            ProductImages = oi.Product.ProductImages?.Select(pi => new ProductImageDto
                            {
                                Id = pi.Id,
                                UrlImage = pi.UrlImage
                            }).ToList() ?? new List<ProductImageDto>(),
                            CartDto = null
                        }
                    }).ToList() ?? new List<OrderItemDto>(),
                    User = null // Avoid cycle
                };

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

                foreach (var orderItem in order.OrderItems)
                {
                    orderItem.Product.Quantity += orderItem.Count;
                    orderItem.Product.Sold -= orderItem.Count;
                    _context.Products.Update(orderItem.Product);
                }

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

                var orders = _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.ProductImages)
                    .Where(o => o.UserId == user_id && o.Status == status)
                    .OrderByDescending(o => o.BookingDate)
                    .ToList();

                var orderDtos = orders.Select(o => new OrderDto
                {
                    Id = o.Id,
                    Total = (int)o.Total,
                    BookingDate = o.BookingDate,
                    PaymentMethod = o.PaymentMethod,
                    Status = o.Status,
                    Fullname = o.Fullname,
                    Country = o.Country,
                    Address = o.Address,
                    Phone = o.Phone,
                    Email = o.Email,
                    Note = o.Note,
                    OrderItems = o.OrderItems?.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        Count = (int)oi.Count,
                        Product = new ProductDto
                        {
                            Id = oi.Product.Id,
                            ProductName = oi.Product.ProductName,
                            Description = oi.Product.Description,
                            Sold = (int)oi.Product.Sold,
                            IsActive = (int)oi.Product.IsActive,
                            IsSelling = (int)oi.Product.IsSelling,
                            CreatedAt = oi.Product.CreatedAt,
                            Price = oi.Product.Price ?? 0,
                            Quantity = (int)oi.Product.Quantity,
                            ProductImages = oi.Product.ProductImages?.Select(pi => new ProductImageDto
                            {
                                Id = pi.Id,
                                UrlImage = pi.UrlImage
                            }).ToList() ?? new List<ProductImageDto>(),
                            CartDto = null
                        }
                    }).ToList() ?? new List<OrderItemDto>(),
                    User = null // Avoid cycle
                }).ToList();

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

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    Total = (int)order.Total,
                    BookingDate = order.BookingDate,
                    PaymentMethod = order.PaymentMethod,
                    Status = order.Status,
                    Fullname = order.Fullname,
                    Country = order.Country,
                    Address = order.Address,
                    Phone = order.Phone,
                    Email = order.Email,
                    Note = order.Note,
                    OrderItems = order.OrderItems?.Select(oi => new OrderItemDto
                    {
                        Id = oi.Id,
                        Count = (int)oi.Count,
                        Product = new ProductDto
                        {
                            Id = oi.Product.Id,
                            ProductName = oi.Product.ProductName,
                            Description = oi.Product.Description,
                            Sold = (int)oi.Product.Sold,
                            IsActive = (int)oi.Product.IsActive,
                            IsSelling = (int)oi.Product.IsSelling,
                            CreatedAt = oi.Product.CreatedAt,
                            Price = oi.Product.Price ?? 0,
                            Quantity = (int)oi.Product.Quantity,
                            ProductImages = oi.Product.ProductImages?.Select(pi => new ProductImageDto
                            {
                                Id = pi.Id,
                                UrlImage = pi.UrlImage
                            }).ToList() ?? new List<ProductImageDto>(),
                            CartDto = null
                        }
                    }).ToList() ?? new List<OrderItemDto>(),
                    User = null // Avoid cycle
                };

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
