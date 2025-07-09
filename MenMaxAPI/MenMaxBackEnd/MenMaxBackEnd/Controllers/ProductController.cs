using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenMaxBackEnd.Models;

namespace MenMaxBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly MenMaxContext _context;

        public ProductController(MenMaxContext context)
        {
            _context = context;
        }

        [HttpGet("newproduct")]
        public ActionResult<List<Product>> NewProduct()
        {
            // Lấy top 12 sản phẩm mới nhất theo ngày tạo
            var newProducts = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == 1 && p.IsSelling == 1)
                .OrderByDescending(p => p.CreatedAt)
                .Take(12)
                .ToList();

            return Ok(newProducts);
        }

        [HttpGet("bestsellers")]
        public ActionResult<List<Product>> BestSellers()
        {
            // Lấy top 12 sản phẩm bán chạy nhất theo số lượng đã bán
            var bestSellers = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == 1 && p.IsSelling == 1)
                .OrderByDescending(p => p.Sold)
                .Take(12)
                .ToList();

            return Ok(bestSellers);
        }

        [HttpGet("search")]
        public ActionResult<List<Product>> Search(string searchContent)
        {
            if (string.IsNullOrEmpty(searchContent))
            {
                return BadRequest("Search content cannot be empty");
            }

            // Tìm kiếm sản phẩm theo tên có chứa từ khóa
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == 1 &&
                           p.IsSelling == 1 &&
                           p.ProductName.Contains(searchContent))
                .OrderBy(p => p.ProductName)
                .ToList();

            return Ok(products);
        }

        // Các method bổ sung từ ProductService interface
        [HttpGet]
        public ActionResult<List<Product>> GetAllProducts()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == 1)
                .OrderBy(p => p.ProductName)
                .ToList();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> SaveProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product data is required");
            }

            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                // Tải lại product với related data
                var savedProduct = _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .FirstOrDefault(p => p.Id == product.Id);

                return Ok(savedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving product: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Product> UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null || id != product.Id)
            {
                return BadRequest("Invalid product data");
            }

            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            try
            {
                // Cập nhật các thuộc tính
                existingProduct.ProductName = product.ProductName;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                existingProduct.IsActive = product.IsActive;
                existingProduct.IsSelling = product.IsSelling;
                existingProduct.CategoryId = product.CategoryId;

                _context.Products.Update(existingProduct);
                _context.SaveChanges();

                // Tải lại product với related data
                var updatedProduct = _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .FirstOrDefault(p => p.Id == id);

                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating product: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            try
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                return Ok($"Product with ID {id} has been deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting product: {ex.Message}");
            }
        }

        [HttpGet("category/{categoryId}")]
        public ActionResult<List<Product>> GetTop4ProductsByCategory(int categoryId)
        {
            // Lấy top 4 sản phẩm theo category
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.CategoryId == categoryId &&
                           p.IsActive == 1 &&
                           p.IsSelling == 1)
                .OrderByDescending(p => p.Sold)
                .Take(4)
                .ToList();

            return Ok(products);
        }

        [HttpGet("search/category")]
        public ActionResult<List<Product>> SearchByNameAndCategory(string name, int categoryId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Search name cannot be empty");
            }

            // Tìm kiếm sản phẩm theo tên và category
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.ProductName.Contains(name) &&
                           p.CategoryId == categoryId &&
                           p.IsActive == 1 &&
                           p.IsSelling == 1)
                .OrderBy(p => p.ProductName)
                .ToList();

            return Ok(products);
        }

        // Pagination support
        [HttpGet("paged")]
        public ActionResult<object> GetProductsPaged(int page = 1, int pageSize = 10)
        {
            var totalProducts = _context.Products.Count(p => p.IsActive == 1);
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == 1)
                .OrderBy(p => p.ProductName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new
            {
                Products = products,
                TotalCount = totalProducts,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize)
            };

            return Ok(result);
        }

        [HttpGet("search/paged")]
        public ActionResult<object> SearchProductsPaged(string name, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Search name cannot be empty");
            }

            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.ProductName.Contains(name) &&
                           p.IsActive == 1 &&
                           p.IsSelling == 1);

            var totalProducts = query.Count();
            var products = query
                .OrderBy(p => p.ProductName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = new
            {
                Products = products,
                TotalCount = totalProducts,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize),
                SearchTerm = name
            };

            return Ok(result);
        }
    }
}
