using backend.Model;
using backend.DTO;


namespace backend.Interface;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync();

    Task<Product?> GetProductByIdAsync(int id);

    Task<List<ProductDto>> GetProductsByCategoryAsync(int id);

    Task CreateProduct(ProductDto dto);

    void UpdateProduct(Product product);

    Task DeleteProduct(int id);
}