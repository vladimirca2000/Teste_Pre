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
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IJwtTokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthenticationService authService,
            IJwtTokenService tokenService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _tokenService = tokenService;
            _logger = logger;
        }

        /// <summary>
        /// Fazer login com credenciais
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation($"Tentativa de login para usuário: {request.Username}");

            var (success, token, message) = await _authService.LoginAsync(request.Username, request.Password);

            if (!success)
            {
                _logger.LogWarning($"Falha no login para usuário: {request.Username}");
                return Unauthorized(new LoginResponse
                {
                    Success = false,
                    Message = message
                });
            }

            var expirationMinutes = 60;
            var userInfo = new UserInfo
            {
                Username = request.Username,
                Role = _tokenService.GetRoleFromToken(token!),
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes)
            };

            _logger.LogInformation($"Login bem-sucedido para usuário: {request.Username}");

            return Ok(new LoginResponse
            {
                Success = true,
                Message = message,
                Token = token,
                ExpiresIn = expirationMinutes * 60,
                User = userInfo
            });
        }

        /// <summary>
        /// Registrar novo usuário
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] LoginRequest request)
        {
            _logger.LogInformation($"Tentativa de registro para usuário: {request.Username}");

            var (success, message) = await _authService.RegisterAsync(request.Username, request.Username, request.Password);

            if (!success)
            {
                _logger.LogWarning($"Falha no registro para usuário: {request.Username}. Motivo: {message}");
                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = message
                });
            }

            _logger.LogInformation($"Registro bem-sucedido para usuário: {request.Username}");

            return Created(string.Empty, new LoginResponse
            {
                Success = true,
                Message = message
            });
        }

        /// <summary>
        /// Alterar senha do usuário autenticado
        /// </summary>
        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<object>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            _logger.LogInformation("Tentativa de alteração de senha");

            // Validar senhas
            if (string.IsNullOrWhiteSpace(request.CurrentPassword) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest(new { success = false, message = "Campos obrigatórios não preenchidos" });
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new { success = false, message = "As senhas não coincidem" });
            }

            if (request.NewPassword.Length < 6)
            {
                return BadRequest(new { success = false, message = "Nova senha deve ter no mínimo 6 caracteres" });
            }

            // Obter userId do token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim?.Value, out var userId))
            {
                _logger.LogWarning("Falha ao obter ID do usuário do token");
                return Unauthorized(new { success = false, message = "Token inválido" });
            }

            var success = await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);

            if (!success)
            {
                _logger.LogWarning($"Falha ao alterar senha para usuário ID {userId}");
                return BadRequest(new { success = false, message = "Falha ao alterar senha. Verifique sua senha atual." });
            }

            _logger.LogInformation($"Senha alterada com sucesso para usuário ID {userId}");
            return Ok(new { success = true, message = "Senha alterada com sucesso" });
        }

        /// <summary>
        /// Validar token JWT
        /// </summary>
        [HttpPost("validate-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> ValidateToken([FromBody] string token)
        {
            var isValid = _tokenService.ValidateToken(token);

            if (!isValid)
            {
                return Unauthorized(new { valid = false, message = "Token inválido ou expirado" });
            }

            var username = _tokenService.GetUsernameFromToken(token);
            var role = _tokenService.GetRoleFromToken(token);

            return Ok(new
            {
                valid = true,
                username,
                role,
                message = "Token válido"
            });
        }
    }
}
