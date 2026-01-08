using MeuTeste.Domain.DTOs;
using MeuTeste.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MeuTeste.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IAuthenticationService authService,
            ILogger<UsersController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Obter usuários inativos (pendentes de aprovação) - Apenas Admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("inactive")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetInactiveUsers()
        {
            _logger.LogInformation("Admin solicitando lista de usuários inativos");

            try
            {
                var inactiveUsers = await _authService.GetInactiveUsersAsync();
                var userDtos = inactiveUsers.Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role.ToString(),
                    IsApproved = u.IsApproved,
                    CreatedAt = u.CreatedAt
                });

                _logger.LogInformation($"Retornando {userDtos.Count()} usuários inativos");
                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar usuários inativos");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Erro ao buscar usuários inativos" });
            }
        }

        /// <summary>
        /// Aprovar usuário - Apenas Admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> ApproveUser(int id)
        {
            _logger.LogInformation($"Admin solicitando aprovação de usuário ID {id}");

            if (id <= 0)
            {
                return BadRequest(new { message = "ID de usuário inválido" });
            }

            try
            {
                var success = await _authService.ApproveUserAsync(id);

                if (!success)
                {
                    _logger.LogWarning($"Usuário ID {id} não encontrado");
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                _logger.LogInformation($"Usuário ID {id} aprovado com sucesso");
                return Ok(new { success = true, message = "Usuário aprovado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao aprovar usuário ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro ao aprovar usuário" });
            }
        }

        /// <summary>
        /// Obter informações do usuário autenticado
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            _logger.LogInformation("Usuário solicitando informações de perfil");

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim?.Value, out var userId))
                {
                    _logger.LogWarning("Falha ao obter ID do usuário do token");
                    return Unauthorized(new { message = "Token inválido" });
                }

                var usernameClaim = User.FindFirst(ClaimTypes.Name);
                var roleClaim = User.FindFirst(ClaimTypes.Role);

                var currentUser = new UserDto
                {
                    Id = userId,
                    Username = usernameClaim?.Value ?? string.Empty,
                    Email = usernameClaim?.Value ?? string.Empty,
                    Role = roleClaim?.Value ?? "User",
                    IsApproved = true,
                    CreatedAt = DateTime.UtcNow
                };

                _logger.LogInformation($"Informações do usuário {userId} retornadas");
                return Ok(currentUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar informações do usuário");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro ao buscar informações do usuário" });
            }
        }
    }
}
