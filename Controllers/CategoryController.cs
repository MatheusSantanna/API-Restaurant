using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetCategories()
    {
        var categories = await _categoryService.GetAllCategories();

        return Ok(categories);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        var newCategory = await _categoryService.CreateCategory(category);

        return Ok(newCategory);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryById(id);

        return Ok(category);
    }
}