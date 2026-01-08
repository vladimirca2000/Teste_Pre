using MeuTeste.Domain.Entities;

namespace MeuTeste.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<(bool Success, string? Token, string? Message)> LoginAsync(string username, string password);
        Task<(bool Success, string? Message)> RegisterAsync(string username, string email, string password);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ApproveUserAsync(int userId);
        Task<IEnumerable<User>> GetInactiveUsersAsync();
    }
}
