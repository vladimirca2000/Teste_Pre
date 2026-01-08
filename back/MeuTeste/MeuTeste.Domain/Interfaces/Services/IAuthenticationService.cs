namespace MeuTeste.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<(bool Success, string? Token, string? Message)> LoginAsync(string username, string password);
        Task<(bool Success, string? Message)> RegisterAsync(string username, string email, string password);
    }
}
