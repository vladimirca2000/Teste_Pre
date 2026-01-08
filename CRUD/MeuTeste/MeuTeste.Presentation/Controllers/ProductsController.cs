using MeuTeste.Application.Interfaces;
using MeuTeste.Domain.Common;
using MeuTeste.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeuTeste.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Obter todos os produtos (sem autenticação)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            _logger.LogInformation("Buscando todos os produtos");
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Obter produtos com paginação e filtros (sem autenticação)
        /// </summary>
        [HttpGet("paginated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetProductsPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? categoryId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            _logger.LogInformation($"Buscando produtos com paginação - Página: {pageNumber}, Tamanho: {pageSize}");
            
            var result = await _productService.GetProductsWithPaginationAsync(
                pageNumber,
                pageSize,
                categoryId,
                minPrice,
                maxPrice,
                sortBy,
                isDescending);

            return Ok(result);
        }

        /// <summary>
        /// Obter produto por ID (sem autenticação)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            _logger.LogInformation($"Buscando produto com ID: {id}");
            var product = await _productService.GetProductByIdAsync(id);
            
            if (product == null)
            {
                _logger.LogWarning($"Produto com ID {id} não encontrado");
                return NotFound(new { message = "Produto não encontrado" });
            }

            return Ok(product);
        }

        /// <summary>
        /// Obter produtos por categoria (sem autenticação)
        /// </summary>
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
        {
            _logger.LogInformation($"Buscando produtos da categoria: {categoryId}");
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        /// <summary>
        /// Obter produtos por faixa de preço (sem autenticação)
        /// </summary>
        [HttpGet("price-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByPriceRange(
            [FromQuery] decimal minPrice,
            [FromQuery] decimal maxPrice)
        {
            _logger.LogInformation($"Buscando produtos entre R$ {minPrice} e R$ {maxPrice}");
            var products = await _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            return Ok(products);
        }

        /// <summary>
        /// Criar novo produto (requer autenticação)
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductInputDto productInputDto)
        {
            _logger.LogInformation($"Criando novo produto: {productInputDto.Name}");
            var product = await _productService.CreateProductAsync(productInputDto);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Atualizar produto (requer autenticação)
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductInputDto productInputDto)
        {
            _logger.LogInformation($"Atualizando produto com ID: {id}");
            var result = await _productService.UpdateProductAsync(id, productInputDto);
            
            if (!result)
            {
                _logger.LogWarning($"Falha ao atualizar produto com ID {id}");
                return NotFound(new { message = "Produto não encontrado" });
            }

            return NoContent();
        }

        /// <summary>
        /// Deletar produto (requer autenticação)
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation($"Deletando produto com ID: {id}");
            var result = await _productService.DeleteProductAsync(id);
            
            if (!result)
            {
                _logger.LogWarning($"Falha ao deletar produto com ID {id}");
                return NotFound(new { message = "Produto não encontrado" });
            }

            return NoContent();
        }
    }
}
