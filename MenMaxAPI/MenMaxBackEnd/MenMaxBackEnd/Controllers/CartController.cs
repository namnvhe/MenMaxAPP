using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenMaxBackEnd.Models;
using MenMaxBackEnd.Services;

namespace MenMaxBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly MenMaxContext _context;
        private readonly ModelMapper _modelMapper;

        public CartController(MenMaxContext context, ModelMapper modelMapper)
        {
            _context = context;
            _modelMapper = modelMapper;
        }

        [HttpPost("addtocart")]
        public ActionResult<CartDto> AddToCart([FromForm] string user_id, [FromForm] int product_id, [FromForm] int count)
        {
            Console.WriteLine($"{user_id}{product_id}{count}");

            var user = _context.Users.FirstOrDefault(u => u.Id == user_id && u.Role == "user");
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var listCart = _context.Carts
                .Include(c => c.Product)
                .Include(c => c.User)
                .Where(c => c.UserId == user_id)
                .ToList();

            var product = _context.Products.FirstOrDefault(p => p.Id == product_id);
            if (product == null)
            {
                return BadRequest("Product not found");
            }

            int flag = 0;
            Cart cart = new Cart();

            foreach (var y in listCart)
            {
                if (y.Product != null && y.Product.Id == product_id)
                {
                    y.Count = y.Count + count;
                    _context.Carts.Update(y);
                    _context.SaveChanges();
                    cart = y;
                    flag = 1;
                    break;
                }
            }

            if (flag == 0)
            {
                var newCart = new Cart
                {
                    Count = count,
                    ProductId = product_id,
                    UserId = user_id
                };

                _context.Carts.Add(newCart);
                _context.SaveChanges();

                cart = _context.Carts
                    .Include(c => c.Product)
                        .ThenInclude(p => p.ProductImages)
                    .Include(c => c.User)
                    .FirstOrDefault(c => c.Id == newCart.Id);
            }

            // Sử dụng ModelMapper để convert Cart sang CartDto
            var cartDto = _modelMapper.Map<Cart, CartDto>(cart);
            return Ok(cartDto);
        }

        [HttpGet("cartofuser")]
        public ActionResult<List<CartDto>> CartOfUser([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required");
            }

            var listCart = _context.Carts
                .Include(c => c.Product)
                    .ThenInclude(p => p.ProductImages)
                .Include(c => c.User)
                .Where(c => c.UserId == userId)
                .ToList();

            // Sử dụng ModelMapper để convert List<Cart> sang List<CartDto>
            var listCartDto = _modelMapper.Map<List<Cart>, List<CartDto>>(listCart);

            foreach (var cartDto in listCartDto)
            {
                Console.WriteLine(cartDto);
            }

            return Ok(listCartDto);
        }

        [HttpPost("cartofuser")]
        public ActionResult<List<CartDto>> CartOfUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Valid user is required");
            }

            return CartOfUser(user.Id);
        }

        [HttpPost("deletecart")]
        public ActionResult<string> DeleteCart([FromForm] int cart_id, [FromForm] string user_id)
        {
            Console.WriteLine($"{cart_id}{user_id}");

            var carts = _context.Carts.Where(c => c.UserId == user_id).ToList();

            foreach (var cart in carts)
            {
                if (cart_id == cart.Id)
                {
                    _context.Carts.Remove(cart);
                    _context.SaveChanges();
                    break;
                }
            }

            return Ok("successfully");
        }
    }
}
