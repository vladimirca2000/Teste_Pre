using MeuTeste.Domain.Enums;

namespace MeuTeste.Domain.Entities
{
    public class User : Base
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.User;
        public bool IsActive { get; set; } = true;
    }
}
