using MeuTeste.Domain.Common;
using MeuTeste.Domain.DTOs;

namespace MeuTeste.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CategoryInputDto categoryInputDto);
        Task<bool> UpdateCategoryAsync(int id, CategoryInputDto categoryInputDto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
