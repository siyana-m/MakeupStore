using Makeup.Services;
using Makeup.Services.DTO.Orders;
using Makeup.Services.DTO.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Makeup.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsService _productsService;
        public ProductsController(ProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productsService.GetAll();
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "operator, admin")]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var createdProduct = await _productsService.Create(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id },
           createdProduct);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productsService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "operator, admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            var updatedProduct = await _productsService.Update(id, productDto);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("{id}/image")]
        [Authorize(Roles = "operator, admin")]
        public async Task<ActionResult<ProductDto>> UploadImage(int id, IFormFile image)
        {
            if (image == null)
            {
                return BadRequest("Image is required.");
            }
            if (image.Length == 0)
            {
                return BadRequest("Image is empty.");
            }
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }
            var updatedProduct = await _productsService.UpdateImage(id, imageData);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var isDeleted = await _productsService.Delete(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        
        [HttpGet("{id}/purchases")]
        [Authorize(Roles = "operator, admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetProductPurchases(int
        id)
        {
            var purchases = await _productsService.GetProductPurchases(id);
            if (purchases == null)
            {
                return NotFound();
            }
            return Ok(purchases);
        }
        
        [HttpGet("revenue-summary")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<RevenueSummaryDto>>> GetRevenueSummary()
        {
            var revenueSummary = await _productsService.GetRevenueSummary();
            return Ok(revenueSummary);
        }

    }
}
