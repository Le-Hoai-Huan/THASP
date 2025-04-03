using Microsoft.AspNetCore.Mvc;
using WebBanHangRestApi.model;
using WebBanHangRestApi.Repository;

namespace WebBanHangRestApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : Controller
    {

        private readonly IProductRepository _productRepository;

        public ProductApiController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {

            var products = await _productRepository.GetProductsAsync();
            return Ok(products);

        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await _productRepository.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            var exsistingProduct = await _productRepository.GetProductByIdAsync(id);
            if (exsistingProduct == null)
            {
                return NotFound();
            }
            exsistingProduct.Name = product.Name;
            exsistingProduct.Price = product.Price;
            exsistingProduct.Description = product.Description;

            await _productRepository.UpdateProductAsync(exsistingProduct);
            return Ok(exsistingProduct);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeteleProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productRepository.DeleteProductAsync(id);
            return Ok(product);


        }
    }
}
