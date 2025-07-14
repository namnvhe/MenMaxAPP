using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenMaxBackEnd.Models;
using AutoMapper;

namespace MenMaxBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly MenMaxContext _context;
        private readonly IMapper _mapper;

        public ProductController(MenMaxContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("newproduct")]
        public ActionResult<List<ProductDto>> NewProduct()
        {
            // Lấy top 12 sản phẩm mới nhất theo ngày tạo
            var newProducts = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == 1 && p.IsSelling == 1)
                .OrderByDescending(p => p.CreatedAt)
                .Take(12)
                .ToList();

            var productDtos = _mapper.Map<List<ProductDto>>(newProducts);
            return Ok(productDtos);
        }

        [HttpGet("bestsellers")]
        public ActionResult<List<ProductDto>> BestSellers()
        {
            // Lấy top 12 sản phẩm bán chạy nhất theo số lượng đã bán
            var bestSellers = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == 1 && p.IsSelling == 1)
                .OrderByDescending(p => p.Sold)
                .Take(12)
                .ToList();

            var productDtos = _mapper.Map<List<ProductDto>>(bestSellers);
            return Ok(productDtos);
        }

        [HttpGet("search")]
        public ActionResult<List<ProductDto>> Search(string? searchContent)
        {
          

            if (string.IsNullOrEmpty(searchContent))
            {
                var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == 1)
                .OrderBy(p => p.ProductName)
                .ToList();

                var productDto = _mapper.Map<List<ProductDto>>(product);
                return Ok(productDto);
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

            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return Ok(productDtos);
        }

        [HttpGet("LoadAll")]
        public ActionResult<List<ProductDto>> LoadAll()
        {



                var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == 1)
                .OrderBy(p => p.ProductName)
                .ToList();

                var productDto = _mapper.Map<List<ProductDto>>(product);
                return Ok(productDto);
            

        }
        [HttpGet]
        public ActionResult<List<ProductDto>> GetAllProducts()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Where(p => p.IsActive == 1)
                .OrderBy(p => p.ProductName)
                .ToList();

            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProductById(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public ActionResult<ProductDto> SaveProduct([FromBody] ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is required");
            }

            try
            {
                var product = _mapper.Map<Product>(productDto);
                _context.Products.Add(product);
                _context.SaveChanges();

                // Tải lại product với related data
                var savedProduct = _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .FirstOrDefault(p => p.Id == product.Id);

                var savedProductDto = _mapper.Map<ProductDto>(savedProduct);
                return Ok(savedProductDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving product: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<ProductDto> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (productDto == null || id != productDto.Id)
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
                // Map từ DTO sang entity, nhưng chỉ cập nhật các thuộc tính cần thiết
                _mapper.Map(productDto, existingProduct);

                _context.Products.Update(existingProduct);
                _context.SaveChanges();

                // Tải lại product với related data
                var updatedProduct = _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .FirstOrDefault(p => p.Id == id);

                var updatedProductDto = _mapper.Map<ProductDto>(updatedProduct);
                return Ok(updatedProductDto);
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
        public ActionResult<List<ProductDto>> GetTop4ProductsByCategory(int categoryId)
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

            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return Ok(productDtos);
        }

        [HttpGet("search/category")]
        public ActionResult<List<ProductDto>> SearchByNameAndCategory(string name, int categoryId)
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

            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return Ok(productDtos);
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

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            var result = new
            {
                Products = productDtos,
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

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            var result = new
            {
                Products = productDtos,
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
