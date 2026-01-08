using MeuTeste.Domain.Entities;
using MeuTeste.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MeuTeste.Infrastructure.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username && !u.IsDelete);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email && !u.IsDelete);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _dbSet.AnyAsync(u => u.Username == username && !u.IsDelete);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && !u.IsDelete);
        }

        public async Task<IEnumerable<User>> GetInactiveUsersAsync()
        {
            return await _dbSet
                .Where(u => !u.IsApproved && u.IsActive && !u.IsDelete)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }
    }
}
