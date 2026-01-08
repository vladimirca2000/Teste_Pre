using MeuTeste.Application.Interfaces;
using MeuTeste.Application.Mappers;
using MeuTeste.Domain.Common;
using MeuTeste.Domain.DTOs;
using MeuTeste.Domain.Entities;
using MeuTeste.Domain.Interfaces.UnitOfWork;
using System.Linq.Expressions;

namespace MeuTeste.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetProductsWithCategoryAsync();
            return products.Where(p => !p.IsDelete).Select(p => p.ToProductDto());
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null || product.IsDelete)
                return null;

            return product.ToProductDto();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.Products.GetByCategoryIdAsync(categoryId);
            return products.Select(p => p.ToProductDto());
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var products = await _unitOfWork.Products.GetByPriceRangeAsync(minPrice, maxPrice);
            return products.Select(p => p.ToProductDto());
        }

        public async Task<PaginatedResult<ProductDto>> GetProductsWithPaginationAsync(
            int pageNumber,
            int pageSize,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? sortBy = null,
            bool isDescending = false)
        {
            // Validar paginação
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            // Construir filtro
            Expression<Func<Product, bool>>? filter = null;

            if (categoryId.HasValue)
            {
                filter = p => p.CategoryId == categoryId.Value;
            }

            if (minPrice.HasValue || maxPrice.HasValue)
            {
                var minP = minPrice ?? 0;
                var maxP = maxPrice ?? decimal.MaxValue;

                filter = filter == null
                    ? p => p.Price >= minP && p.Price <= maxP
                    : (p => filter.Compile().Invoke(p) && p.Price >= minP && p.Price <= maxP);
            }

            // Construir ordenação
            Expression<Func<Product, object>>? orderBy = sortBy?.ToLower() switch
            {
                "name" => p => p.Name,
                "price" => p => p.Price,
                "category" => p => p.CategoryId,
                _ => p => p.Id
            };

            // Obter dados
            var (items, total) = await _unitOfWork.Products.GetProductsWithPaginationAsync(
                pageNumber,
                pageSize,
                filter,
                orderBy,
                isDescending);

            return new PaginatedResult<ProductDto>
            {
                Items = items.Select(p => p.ToProductDto()),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = total
            };
        }

        public async Task<ProductDto> CreateProductAsync(ProductInputDto productInputDto)
        {
            var product = productInputDto.ToProductEntity();
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            // Recarregar com a categoria
            await _unitOfWork.Products.GetByIdAsync(product.Id);
            return product.ToProductDto();
        }

        public async Task<bool> UpdateProductAsync(int id, ProductInputDto productInputDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null || product.IsDelete)
                return false;

            productInputDto.UpdateProductEntity(product);
            _unitOfWork.Products.Update(product);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null || product.IsDelete)
                return false;

            product.IsDelete = true;
            _unitOfWork.Products.Update(product);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
