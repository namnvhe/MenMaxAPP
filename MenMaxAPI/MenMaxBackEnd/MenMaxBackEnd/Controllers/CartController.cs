using MenMaxBackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using MenMaxBackEnd.ModelsDTO;
using Microsoft.EntityFrameworkCore;

namespace MenMaxBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly MenMaxContext _context;

        public CartController(MenMaxContext context)
        {
            _context = context;
        }


        [HttpPost("addtocart")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult AddToCart([FromForm] string user_id, [FromForm] int product_id, [FromForm] int count)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == user_id && u.Role == "user");
            if (user == null)
                return NotFound("User not found");

            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == product_id);
            if (product == null)
                return NotFound("Product not found");

            var existingCart = _context.Carts
                .FirstOrDefault(c => c.UserId == user_id && c.ProductId == product_id);

            Cart cart;
            if (existingCart != null)
            {
                existingCart.Count += count;
                _context.Carts.Update(existingCart);
                cart = existingCart;
            }
            else
            {
                cart = new Cart
                {
                    Count = count,
                    ProductId = product_id,
                    UserId = user_id
                };
                _context.Carts.Add(cart);
            }

            _context.SaveChanges();

            // Manual mapping from Cart to CartDto
            var cartDto = new CartDto
            {
                Id = cart.Id,
                CategoryName = product.Category?.CategoryName ?? string.Empty,
                CategoryImage = product.Category?.CategoryImage ?? string.Empty,
                Product = new List<ProductDto>
                {
                    new ProductDto
                    {
                        Id = product.Id,
                        ProductName = product.ProductName ?? string.Empty,
                        Description = product.Description ?? string.Empty,
                        Sold = product.Sold,
                        IsActive = product.IsActive,
                        IsSelling = product.IsSelling,
                        CreatedAt = product.CreatedAt,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        ProductImage = product.ProductImages?.Select(img => new ProductImageDto
                        {
                            id = img.Id,
                            imageUrl = img.UrlImage ?? string.Empty
                        }).ToList() ?? new List<ProductImageDto>(),
                        CartDto = null // Avoid circular reference
                    }
                }
            };

            return Ok(cartDto);
        }

        [HttpGet("cartofuser")]
        public IActionResult CartOfUser([FromQuery] string userId)
        {
            // Eagerly load Product, Category, and ProductImages
            var carts = _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Category)
                .Include(c => c.Product)
                    .ThenInclude(p => p.ProductImages)
                .ToList();

            // Manual mapping from Cart to CartDto
            var cartDtos = new List<CartDto>();
            foreach (var cart in carts)
            {
                DateOnly? createdAt = cart.Product.CreatedAt;
                var cartDto = new CartDto
                {
                    Id = cart.Id,
                    CategoryName = cart.Product?.Category?.CategoryName ?? string.Empty,
                    CategoryImage = cart.Product?.Category?.CategoryImage ?? string.Empty,
                    Product = cart.Product != null ? new List<ProductDto>
                    {
                        new ProductDto
                        {
                            Id = cart.Product.Id,
                            ProductName = cart.Product.ProductName ?? string.Empty,
                            Description = cart.Product.Description ?? string.Empty,
                            Sold = cart.Product.Sold,
                            IsActive = cart.Product.IsActive,
                            IsSelling = cart.Product.IsSelling,
                            CreatedAt =createdAt,
                            Price = cart.Product.Price,
                            Quantity = cart.Product.Quantity,
                            ProductImage = cart.Product.ProductImages?.Select(img => new ProductImageDto
                            {
                                id = img.Id,
                                imageUrl = img.UrlImage ?? string.Empty
                            }).ToList() ?? new List<ProductImageDto>(),
                            CartDto = null 
                        }
                    } : new List<ProductDto>()
                };
                cartDtos.Add(cartDto);
            }

            return Ok(cartDtos);
        }

        [HttpPost("deletecart")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult DeleteCart([FromForm] int cart_id, [FromForm] string user_id)
        {
            var cart = _context.Carts
                .FirstOrDefault(c => c.Id == cart_id && c.UserId == user_id);

            if (cart == null)
                return NotFound("Cart not found");

            _context.Carts.Remove(cart);
            _context.SaveChanges();
            return Ok("successfully");
        }
    }
}