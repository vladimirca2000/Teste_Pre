using MeuTeste.Domain.Enums;
using MeuTeste.Domain.Interfaces.Services;
using MeuTeste.Domain.Interfaces.UnitOfWork;
using MeuTeste.Domain.Services;

namespace MeuTeste.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticationService(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<(bool Success, string? Token, string? Message)> LoginAsync(string username, string password)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(username);

            if (user == null || !user.IsActive)
                return (false, null, "Usuário ou senha inválidos.");

            // Verificar se usuário foi aprovado
            if (!user.IsApproved)
                return (false, null, "Usuário aguardando aprovação do administrador.");

            // Verificar senha com hash duplo
            if (!PasswordHasher.VerifyPassword(password, username, user.PasswordHash))
                return (false, null, "Usuário ou senha inválidos.");

            // Atualizar timestamp
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedUser = username;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            var token = _jwtTokenService.GenerateToken(user.Username, user.Role.ToString());
            return (true, token, "Login realizado com sucesso.");
        }

        public async Task<(bool Success, string? Message)> RegisterAsync(string username, string email, string password)
        {
            if (await _unitOfWork.Users.UsernameExistsAsync(username))
                return (false, "Nome de usuário já existe.");

            if (await _unitOfWork.Users.EmailExistsAsync(email))
                return (false, "Email já registrado.");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return (false, "Senha deve ter no mínimo 6 caracteres.");

            // Hash duplo da senha
            var passwordHash = PasswordHasher.HashPassword(password, username);

            var user = new Domain.Entities.User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Role = Role.User,
                IsActive = true,
                IsApproved = false,  // ? Novo: usuário precisa de aprovação
                CreatedUser = username
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Usuário registrado com sucesso. Aguardando aprovação do administrador.");
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
                return false;

            // Verificar senha atual
            if (!PasswordHasher.VerifyPassword(currentPassword, user.Username, user.PasswordHash))
                return false;

            // Gerar novo hash
            var newPasswordHash = PasswordHasher.HashPassword(newPassword, user.Username);
            user.PasswordHash = newPasswordHash;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedUser = user.Username;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApproveUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
                return false;

            user.IsApproved = true;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedUser = "Admin";

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Domain.Entities.User>> GetInactiveUsersAsync()
        {
            return await _unitOfWork.Users.GetInactiveUsersAsync();
        }
    }
}
