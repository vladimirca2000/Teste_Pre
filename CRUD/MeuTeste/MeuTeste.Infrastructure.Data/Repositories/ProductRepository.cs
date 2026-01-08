using MeuTeste.Domain.Entities;
using MeuTeste.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MeuTeste.Infrastructure.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _dbSet
                .Where(p => p.CategoryId == categoryId && !p.IsDelete)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbSet
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice && !p.IsDelete)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
        {
            return await _dbSet
                .Where(p => !p.IsDelete)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Product> Items, int Total)> GetProductsWithPaginationAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<Product, bool>>? filter = null,
            Expression<Func<Product, object>>? orderBy = null,
            bool isDescending = false)
        {
            IQueryable<Product> query = _dbSet.Where(p => !p.IsDelete).Include(p => p.Category);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var total = await query.CountAsync();

            if (orderBy != null)
            {
                query = isDescending
                    ? query.OrderByDescending(orderBy)
                    : query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(p => p.Id);
            }

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
