using MeuTeste.Domain.DTOs;

namespace MeuTeste.Domain.Interfaces.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string username, string role, int expirationMinutes = 60);
        bool ValidateToken(string token);
        string? GetUsernameFromToken(string token);
        string? GetRoleFromToken(string token);
    }
}
