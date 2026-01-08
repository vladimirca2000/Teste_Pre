using MeuTeste.Domain.DTOs;
using MeuTeste.Domain.Entities;

namespace MeuTeste.Application.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public static Category ToCategoryEntity(this CategoryInputDto categoryInputDto)
        {
            return new Category
            {
                Name = categoryInputDto.Name
            };
        }

        public static Category UpdateCategoryEntity(this CategoryInputDto categoryInputDto, Category category)
        {
            category.Name = categoryInputDto.Name;
            return category;
        }
    }
}
