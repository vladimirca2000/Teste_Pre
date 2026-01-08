using MeuTeste.Domain.Common;
using MeuTeste.Domain.DTOs;

namespace MeuTeste.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<PaginatedResult<ProductDto>> GetProductsWithPaginationAsync(
            int pageNumber,
            int pageSize,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? sortBy = null,
            bool isDescending = false);
        Task<ProductDto> CreateProductAsync(ProductInputDto productInputDto);
        Task<bool> UpdateProductAsync(int id, ProductInputDto productInputDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
