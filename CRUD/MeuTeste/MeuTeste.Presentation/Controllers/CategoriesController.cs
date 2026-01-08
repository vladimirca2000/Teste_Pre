using MeuTeste.Application.Interfaces;
using MeuTeste.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeuTeste.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Obter todas as categorias (sem autenticação)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            _logger.LogInformation("Buscando todas as categorias");
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Obter categoria por ID (sem autenticação)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            _logger.LogInformation($"Buscando categoria com ID: {id}");
            var category = await _categoryService.GetCategoryByIdAsync(id);
            
            if (category == null)
            {
                _logger.LogWarning($"Categoria com ID {id} não encontrada");
                return NotFound(new { message = "Categoria não encontrada" });
            }

            return Ok(category);
        }

        /// <summary>
        /// Criar nova categoria (requer autenticação)
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryInputDto categoryInputDto)
        {
            _logger.LogInformation($"Criando nova categoria: {categoryInputDto.Name}");
            var category = await _categoryService.CreateCategoryAsync(categoryInputDto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        /// <summary>
        /// Atualizar categoria (requer autenticação)
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryInputDto categoryInputDto)
        {
            _logger.LogInformation($"Atualizando categoria com ID: {id}");
            var result = await _categoryService.UpdateCategoryAsync(id, categoryInputDto);
            
            if (!result)
            {
                _logger.LogWarning($"Falha ao atualizar categoria com ID {id}");
                return NotFound(new { message = "Categoria não encontrada" });
            }

            return NoContent();
        }

        /// <summary>
        /// Deletar categoria (requer autenticação)
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            _logger.LogInformation($"Deletando categoria com ID: {id}");
            var result = await _categoryService.DeleteCategoryAsync(id);
            
            if (!result)
            {
                _logger.LogWarning($"Falha ao deletar categoria com ID {id}");
                return NotFound(new { message = "Categoria não encontrada" });
            }

            return NoContent();
        }
    }
}
