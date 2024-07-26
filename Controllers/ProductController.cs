using Microsoft.AspNetCore.Mvc;
using StoreAPI.Dtos;
using StoreAPI.Services;

namespace StoreAPI.Controllers
{
    [Route("api/companies/{companyId}/stores/{storeId}/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(long companyId, long storeId, CreateUpdateProductDto productDto)
        {
            var product = await _productService.CreateAsync(companyId, storeId, productDto);
            return CreatedAtAction(nameof(GetProductById), new { companyId, storeId, id = product.Id }, product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(long companyId, long storeId, long id)
        {
            var product = await _productService.GetByIdAsync(companyId, storeId, id);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(long companyId, long storeId, long id, CreateUpdateProductDto updateProductDto)
        {
            await _productService.UpdateAsync(companyId, storeId, id, updateProductDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long companyId, long storeId, long id)
        {
            await _productService.DeleteAsync(companyId, storeId, id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(long companyId, long storeId)
        {
            var products = await _productService.GetAllAsync(companyId, storeId);
            return Ok(products);
        }

        [HttpGet("json")]
        public async Task<IActionResult> GetAllProductsAsJson(long companyId, long storeId)
        {
            var products = await _productService.GetAllProductsAsJsonAsync(companyId, storeId);
            return Ok(products);
        }
    }
}