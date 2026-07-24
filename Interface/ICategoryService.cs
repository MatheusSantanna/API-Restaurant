using backend.Model;

namespace backend.Interface;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategories();

    Task<Category?> GetCategoryById(int id);

    Task<Category> CreateCategory(Category category);
}