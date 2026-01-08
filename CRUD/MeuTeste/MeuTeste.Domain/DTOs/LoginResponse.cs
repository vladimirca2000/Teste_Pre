namespace MeuTeste.Domain.DTOs
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public int? ExpiresIn { get; set; }
        public UserInfo? User { get; set; }
    }

    public class UserInfo
    {
        public string? Username { get; set; }
        public string? Role { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
