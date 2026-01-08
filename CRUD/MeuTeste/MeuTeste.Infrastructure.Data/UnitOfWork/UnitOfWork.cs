using MeuTeste.Domain.Interfaces.Repositories;
using MeuTeste.Domain.Interfaces.UnitOfWork;
using MeuTeste.Infrastructure.Data.Context;
using MeuTeste.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace MeuTeste.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MeuTesteDbContext _context;
        private IDbContextTransaction? _transaction;
        private ICategoryRepository? _categoryRepository;
        private IProductRepository? _productRepository;
        private IUserRepository? _userRepository;

        public UnitOfWork(MeuTesteDbContext context)
        {
            _context = context;
        }

        public ICategoryRepository Categories =>
            _categoryRepository ??= new CategoryRepository(_context);

        public IProductRepository Products =>
            _productRepository ??= new ProductRepository(_context);

        public IUserRepository Users =>
            _userRepository ??= new UserRepository(_context);

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync();
                await _transaction?.CommitAsync()!;
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                }
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                await _transaction?.RollbackAsync()!;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                }
                _transaction = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }

            await _context.DisposeAsync();
        }
    }
}
