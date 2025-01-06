using ProductInventory.Models;

namespace ProductInventory.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryById(int id);
        ServiceResult CreateCategory(Category category);
        ServiceResult UpdateCategory(Category category);
        ServiceResult DeleteCategory(int id);
        bool CategoryExists(string name, int? excludeId = null);
    }
}
