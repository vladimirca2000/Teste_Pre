using System.Security.Cryptography;
using System.Text;

namespace MeuTeste.Domain.Services
{
    public static class PasswordHasher
    {
        private const string SECRET_KEY = "teste123";

        /// <summary>
        /// Cria um hash duplo da senha usando SHA256 + custom hash
        /// </summary>
        public static string HashPassword(string password, string username)
        {
            // Primeiro hash: SHA256 com username
            using (var sha256 = SHA256.Create())
            {
                var combined = $"{password}{username}{SECRET_KEY}";
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                var firstHash = Convert.ToBase64String(hashBytes);

                // Segundo hash: SHA256 do primeiro hash
                var secondHashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(firstHash));
                var finalHash = Convert.ToBase64String(secondHashBytes);

                return finalHash;
            }
        }

        /// <summary>
        /// Verifica se uma senha corresponde ao hash armazenado
        /// </summary>
        public static bool VerifyPassword(string password, string username, string hash)
        {
            var computedHash = HashPassword(password, username);
            return computedHash == hash;
        }
    }
}
