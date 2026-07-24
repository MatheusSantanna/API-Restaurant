using backend.DTO;
using backend.Interface;
using backend.Model;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;


namespace backend.Service;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly ICategoryService _categoryService;

    public ProductService(
        IRepository<Product> productRepository,
        ICategoryService categoryService)
    {
        _productRepository = productRepository;
        _categoryService = categoryService;
    }

    public async Task CreateProduct(ProductDto dto)
    {
        var product = new Product
        {
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price
        };

        await _productRepository.AddAsync(product);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _productRepository
            .GetAllAsync()
            .OrderBy(x => x.Id)
            .ThenBy(x => x.CategoryId)
            .Select(x => new Product
            {
                CategoryId = x.CategoryId,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price
            })
            .ToListAsync();
    }

    public async Task<List<ProductDto>> GetProductsByCategoryAsync(int id)
    {
        return await _productRepository
            .GetAllAsync()
            .Where(x => x.CategoryId == id)
            .Select(x => new ProductDto
            {
                CategoryId = x.CategoryId,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price
            })
            .ToListAsync();
    }

    public void UpdateProduct(Product product)
    {
        _productRepository.Update(product);
    }

    public async Task DeleteProduct(int id)
    {
        await _productRepository.DeleteAsync(id);
    }
}