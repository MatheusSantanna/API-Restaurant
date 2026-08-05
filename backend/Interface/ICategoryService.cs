using backend.DTO;
using backend.Model;

namespace backend.Interface;

public interface ICategoryService
{
    Task<List<CategoryDTO>> GetAllCategories();

    Task<Category?> GetCategoryById(int id);

    Task<Category> CreateCategory(Category category);
}