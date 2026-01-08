using Moq;
using Xunit;
using FluentAssertions;
using MeuTeste.Application.Services;
using MeuTeste.Domain.DTOs;
using MeuTeste.Domain.Entities;
using MeuTeste.Domain.Interfaces.UnitOfWork;
using MeuTeste.Domain.Interfaces.Repositories;

namespace MeuTeste.Tests.Application
{
    public class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockUnitOfWork.Setup(u => u.Categories).Returns(_mockCategoryRepository.Object);
            _service = new CategoryService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_Should_Return_All_Categories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new() { Id = 1, Name = "Category 1", IsDelete = false },
                new() { Id = 2, Name = "Category 2", IsDelete = false }
            };

            _mockCategoryRepository
                .Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _service.GetAllCategoriesAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().NotBeNull();
            _mockCategoryRepository.Verify(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_Should_Return_Category_When_Exists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Test Category", IsDelete = false };
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

            // Act
            var result = await _service.GetCategoryByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result?.Name.Should().Be("Test Category");
            _mockCategoryRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task CreateCategoryAsync_Should_Add_New_Category()
        {
            // Arrange
            var inputDto = new CategoryInputDto { Name = "New Category" };
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            var result = await _service.CreateCategoryAsync(inputDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("New Category");
            _mockCategoryRepository.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_Should_Update_Category_When_Exists()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Old Name", IsDelete = false };
            var inputDto = new CategoryInputDto { Name = "New Name" };

            _mockCategoryRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateCategoryAsync(1, inputDto);

            // Assert
            result.Should().BeTrue();
            _mockCategoryRepository.Verify(r => r.Update(It.IsAny<Category>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_Should_Soft_Delete_Category()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Category", IsDelete = false };
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteCategoryAsync(1);

            // Assert
            result.Should().BeTrue();
            category.IsDelete.Should().BeTrue();
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
