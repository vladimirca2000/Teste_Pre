using MeuTeste.Domain.DTOs;
using MeuTeste.Domain.Entities;

namespace MeuTeste.Application.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Price = product.Price,
                CategoryName = product.Category?.Name
            };
        }

        public static Product ToProductEntity(this ProductInputDto productInputDto)
        {
            return new Product
            {
                Name = productInputDto.Name,
                CategoryId = productInputDto.CategoryId,
                Price = productInputDto.Price
            };
        }

        public static Product UpdateProductEntity(this ProductInputDto productInputDto, Product product)
        {
            product.Name = productInputDto.Name;
            product.CategoryId = productInputDto.CategoryId;
            product.Price = productInputDto.Price;
            return product;
        }
    }
}
