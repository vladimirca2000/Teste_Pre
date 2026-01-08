using MeuTeste.Application.Interfaces;
using MeuTeste.Application.Mappers;
using MeuTeste.Domain.DTOs;
using MeuTeste.Domain.Entities;
using MeuTeste.Domain.Interfaces.UnitOfWork;

namespace MeuTeste.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.FindAsync(c => !c.IsDelete);
            return categories.Select(c => c.ToCategoryDto());
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null || category.IsDelete)
                return null;

            return category.ToCategoryDto();
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryInputDto categoryInputDto)
        {
            var category = categoryInputDto.ToCategoryEntity();
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category.ToCategoryDto();
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryInputDto categoryInputDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null || category.IsDelete)
                return false;

            categoryInputDto.UpdateCategoryEntity(category);
            _unitOfWork.Categories.Update(category);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null || category.IsDelete)
                return false;

            category.IsDelete = true;
            _unitOfWork.Categories.Update(category);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
