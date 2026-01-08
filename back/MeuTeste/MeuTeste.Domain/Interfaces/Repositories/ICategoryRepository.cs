using MeuTeste.Domain.Entities;

namespace MeuTeste.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);
        Task<IEnumerable<Category>> GetCategoriesWithProductsAsync();
    }
}
