using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenMaxBackEnd.Models;
using MenMaxBackEnd.Services;

namespace MenMaxBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly MenMaxContext _context;
        private readonly ModelMapper _modelMapper;

        public CategoryController(MenMaxContext context, ModelMapper modelMapper)
        {
            _context = context;
            _modelMapper = modelMapper;
        }

        [HttpGet("category")]
        public ActionResult<List<CategoryDto>> GetCategory()
        {
            try
            {
                // Lấy tất cả categories với số lượng products
                var categories = _context.Categories
                    .Include(c => c.Products)
                    .OrderBy(c => c.CategoryName)
                    .ToList();

                // Convert sang CategoryDto bằng ModelMapper
                var categoryDtos = _modelMapper.MapList<Category, CategoryDto>(categories);

                return Ok(categoryDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Các method bổ sung hữu ích
        [HttpGet]
        public ActionResult<List<CategoryDto>> GetAllCategories()
        {
            return GetCategory();
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetCategoryById(int id)
        {
            try
            {
                var category = _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }

                var categoryDto = _modelMapper.Map<CategoryDto>(category);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<CategoryDto> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    return BadRequest("Category data is required");
                }

                // Kiểm tra tên category đã tồn tại chưa
                var existingCategory = _context.Categories
                    .FirstOrDefault(c => c.CategoryName == categoryDto.CategoryName);

                if (existingCategory != null)
                {
                    return BadRequest("Category name already exists");
                }

                var category = new Category
                {
                    CategoryName = categoryDto.CategoryName,
                    CategoryImage = categoryDto.CategoryImage
                };

                _context.Categories.Add(category);
                _context.SaveChanges();

                // Reload category với related data
                var savedCategory = _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefault(c => c.Id == category.Id);

                var resultDto = _modelMapper.Map<CategoryDto>(savedCategory);
                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<CategoryDto> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null || id != categoryDto.Id)
                {
                    return BadRequest("Invalid category data");
                }

                var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);
                if (existingCategory == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }

                // Cập nhật thông tin
                existingCategory.CategoryName = categoryDto.CategoryName;
                existingCategory.CategoryImage = categoryDto.CategoryImage;

                _context.Categories.Update(existingCategory);
                _context.SaveChanges();

                // Reload category với related data
                var updatedCategory = _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefault(c => c.Id == id);

                var resultDto = _modelMapper.Map<CategoryDto>(updatedCategory);
                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                var category = _context.Categories
                    .Include(c => c.Products)
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }

                // Kiểm tra xem category có products không
                if (category.Products.Any())
                {
                    return BadRequest("Cannot delete category that has products");
                }

                _context.Categories.Remove(category);
                _context.SaveChanges();

                return Ok($"Category with ID {id} has been deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/products")]
        public ActionResult<List<ProductDto>> GetProductsByCategory(int id)
        {
            try
            {
                var category = _context.Categories
                    .Include(c => c.Products)
                        .ThenInclude(p => p.ProductImages)
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }

                var productDtos = _modelMapper.MapList<Product, ProductDto>(category.Products.ToList());
                return Ok(productDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("active")]
        public ActionResult<List<CategoryDto>> GetActiveCategories()
        {
            try
            {
                // Lấy categories có ít nhất 1 product active
                var categories = _context.Categories
                    .Include(c => c.Products)
                    .Where(c => c.Products.Any(p => p.IsActive == 1))
                    .OrderBy(c => c.CategoryName)
                    .ToList();

                var categoryDtos = _modelMapper.MapList<Category, CategoryDto>(categories);
                return Ok(categoryDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("stats")]
        public ActionResult<object> GetCategoryStats()
        {
            try
            {
                var stats = _context.Categories
                    .Include(c => c.Products)
                    .Select(c => new
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        ProductCount = c.Products.Count,
                        ActiveProductCount = c.Products.Count(p => p.IsActive == 1),
                        TotalSold = c.Products.Sum(p => p.Sold ?? 0)
                    })
                    .OrderByDescending(c => c.ProductCount)
                    .ToList();

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
