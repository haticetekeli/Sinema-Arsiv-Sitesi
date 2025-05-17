using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SinemaArsivSitesi.Models;
using SinemaArsivSitesi.Services.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SinemaArsivSitesi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("categories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            if (categories == null || categories.Count == 0)
            {
                return NotFound("Hiç kategori bulunamadı.");
            }

            return Ok(categories);
        }

        [HttpPost("initialize")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InitializeCategories()
        {
            await _categoryService.InitializeDefaultCategories();  
            return Ok("Kategoriler başarıyla başlatıldı.");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Kategori bilgileri geçersiz.");
            }

            if (string.IsNullOrEmpty(category.Name))
            {
                return BadRequest("Kategori adı boş olamaz.");
            }

            var result = await _categoryService.AddCategory(category.Name, category.Description);
            if (!result)
            {
                return BadRequest("Kategori oluşturulamadı.");
            }

            return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (category == null || id != category.Id)
            {
                return BadRequest("Geçersiz kategori bilgisi.");
            }

            var result = await _categoryService.UpdateCategory(id, category.Name, category.Description);
            if (!result)
            {
                return BadRequest("Kategori güncellenemedi.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound("Kategori bulunamadı.");
            }

            var result = await _categoryService.DeleteCategory(id);
            if (!result)
            {
                return BadRequest("Kategori silinemedi.");
            }

            return NoContent();
        }
    }
}
