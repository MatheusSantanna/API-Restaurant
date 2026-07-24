using backend.DTO;
using backend.Interface;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllProductsAsync();

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
    {
        await _productService.CreateProduct(productDto);

        return Ok(productDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        var existingProduct = await _productService.GetProductByIdAsync(id);

        if (existingProduct == null || existingProduct.Id != id)
        {
            return BadRequest("The provided Id does not match the product Id.");
        }

        _productService.UpdateProduct(product);

        return Ok(product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        return Ok(product);
    }
}