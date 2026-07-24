using backend.Interface;
using backend.Model;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _repository;

    public CategoryService(IRepository<Category> repository)
    {
        _repository = repository;
    }

    public async Task<Category> CreateCategory(Category category)
    {
        await _repository.AddAsync(category);
        return category;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        return await _repository.GetAllAsync().ToListAsync();
    }

    public async Task<Category?> GetCategoryById(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}