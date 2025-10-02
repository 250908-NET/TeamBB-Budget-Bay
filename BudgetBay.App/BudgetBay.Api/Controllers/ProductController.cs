using BudgetBay.Api.DTOs;
using BudgetBay.Models;
using BudgetBay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace BudgetBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService productService, ILogger<ProductController> logger) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly ILogger<ProductController> _logger = logger;


        //Get all products
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //Get specified product by id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product with ID {ProductId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //Create a product
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdProduct = await _productService.CreateProductAsync(product);
                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //Update specified product details
        [HttpPut("{productId}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] UpdateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(productId, productDto);
                return Ok(updatedProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //Delete product
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var existingProduct = await _productService.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }
                var result = await _productService.DeleteProductByIdAsync(id);
                if (!result)
                {
                    return StatusCode(500, "Failed to delete the product.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID {ProductId}", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //Search for products that relate to query
        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchProducts([FromQuery] string q)
        {
            try
            {
                var products = await _productService.SearchProductsAsync(q);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching products with query {Query}", q);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
