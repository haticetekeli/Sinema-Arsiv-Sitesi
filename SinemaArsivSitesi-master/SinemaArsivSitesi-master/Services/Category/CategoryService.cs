using Microsoft.EntityFrameworkCore;
using SinemaArsivSitesi.Data.DbContext;
using SinemaArsivSitesi.Models;

namespace SinemaArsivSitesi.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InitializeDefaultCategories()
        {
            if (!await _context.Categories.AnyAsync())
            {
                var defaultCategories = new List<Models.Category>
                {
                    new Models.Category { Name = "Aksiyon", Description = "Aksiyon filmleri" },
                    new Models.Category { Name = "Dram", Description = "Dram filmleri" },
                    new Models.Category { Name = "Komedi", Description = "Komedi filmleri" }
                };

                _context.Categories.AddRange(defaultCategories);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> AddCategory(string name, string description)
        {
            var category = new Models.Category
            {
                Name = name,
                Description = description,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            _context.Categories.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCategory(int id, string name, string description)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            category.Name = name;
            category.Description = description;
            category.UpdatedDate = DateTime.Now;

            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Models.Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Models.Category?> GetCategoryById(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
