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
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);
            _service = new ProductService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_Should_Return_All_Products()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", CategoryId = 1, Price = 100, IsDelete = false },
                new() { Id = 2, Name = "Product 2", CategoryId = 1, Price = 200, IsDelete = false }
            };

            _mockProductRepository
                .Setup(r => r.GetProductsWithCategoryAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _service.GetAllProductsAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().NotBeNull();
            _mockProductRepository.Verify(r => r.GetProductsWithCategoryAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_Should_Return_Product_When_Exists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", CategoryId = 1, Price = 100, IsDelete = false };
            _mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _service.GetProductByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result?.Name.Should().Be("Test Product");
            result?.Price.Should().Be(100);
        }

        [Fact]
        public async Task CreateProductAsync_Should_Add_New_Product()
        {
            // Arrange
            var inputDto = new ProductInputDto { Name = "New Product", CategoryId = 1, Price = 500 };
            var product = new Product { Id = 1, Name = "New Product", CategoryId = 1, Price = 500 };

            _mockProductRepository.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);
            _mockProductRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);

            // Act
            var result = await _service.CreateProductAsync(inputDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("New Product");
            _mockProductRepository.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductsByPriceRangeAsync_Should_Return_Products_In_Range()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", CategoryId = 1, Price = 150, IsDelete = false },
                new() { Id = 2, Name = "Product 2", CategoryId = 1, Price = 250, IsDelete = false }
            };

            _mockProductRepository
                .Setup(r => r.GetByPriceRangeAsync(100, 300))
                .ReturnsAsync(products);

            // Act
            var result = await _service.GetProductsByPriceRangeAsync(100, 300);

            // Assert
            result.Should().HaveCount(2);
            _mockProductRepository.Verify(r => r.GetByPriceRangeAsync(100, 300), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_Should_Soft_Delete_Product()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product", CategoryId = 1, Price = 100, IsDelete = false };
            _mockProductRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteProductAsync(1);

            // Assert
            result.Should().BeTrue();
            product.IsDelete.Should().BeTrue();
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
