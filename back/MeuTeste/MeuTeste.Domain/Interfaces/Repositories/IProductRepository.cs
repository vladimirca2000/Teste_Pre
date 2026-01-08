using MeuTeste.Domain.Entities;
using System.Linq.Expressions;

namespace MeuTeste.Domain.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Product>> GetProductsWithCategoryAsync();
        Task<(IEnumerable<Product> Items, int Total)> GetProductsWithPaginationAsync(
            int pageNumber, 
            int pageSize, 
            Expression<Func<Product, bool>>? filter = null,
            Expression<Func<Product, object>>? orderBy = null,
            bool isDescending = false);
    }
}
