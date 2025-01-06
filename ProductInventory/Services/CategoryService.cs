using Microsoft.EntityFrameworkCore;
using ProductInventory.Data;
using ProductInventory.Models;

namespace ProductInventory.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories
                .OrderBy(c => c.Name)
                .ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public ServiceResult CreateCategory(Category category)
        {
            try
            {
                if (CategoryExists(category.Name))
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "A category with this name already exists."
                    };
                }

                _context.Categories.Add(category);
                _context.SaveChanges();

                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Error creating category: {ex.Message}"
                };
            }
        }

        public ServiceResult UpdateCategory(Category category)
        {
            try
            {
                if (CategoryExists(category.Name, category.CategoryId))
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "A category with this name already exists."
                    };
                }

                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();

                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Error updating category: {ex.Message}"
                };
            }
        }

        public ServiceResult DeleteCategory(int id)
        {
            try
            {
                var category = _context.Categories.Find(id);

                if (category == null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Category not found."
                    };
                }

                if (_context.Products.Any(p => p.CategoryId == id))
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Cannot delete category that has associated products. Please delete or reassign the products first."
                    };
                }

                _context.Categories.Remove(category);
                _context.SaveChanges();

                return new ServiceResult { Success = true };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Error deleting category: {ex.Message}"
                };
            }
        }

        public bool CategoryExists(string name, int? excludeId = null)
        {
            return _context.Categories.Any(c =>
                c.Name.ToLower() == name.ToLower() &&
                (!excludeId.HasValue || c.CategoryId != excludeId.Value));
        }
    }
}
