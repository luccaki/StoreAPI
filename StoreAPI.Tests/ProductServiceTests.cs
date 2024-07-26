using Moq;
using StoreAPI.Dtos;
using StoreAPI.Models;
using StoreAPI.Repositories;
using StoreAPI.Services;

namespace StoreAPI.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IStoreService> _storeServiceMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _storeServiceMock = new Mock<IStoreService>();
            _productService = new ProductService(_productRepositoryMock.Object, _storeServiceMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedProductDto()
        {
            // Arrange
            var companyId = 1L;
            var storeId = 1L;
            var productDto = new CreateUpdateProductDto("Test Product", 100);
            var product = new Product { Id = 1, Name = "Test Product", Value = 100, StoreId = storeId };

            var store = new StoreDto(storeId, "Test Store", companyId, null);
            _storeServiceMock.Setup(s => s.GetByIdAsync(companyId, storeId)).ReturnsAsync(store);
            _productRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Product>())).ReturnsAsync(product);

            // Act
            var result = await _productService.CreateAsync(companyId, storeId, productDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Value, result.Value);
            Assert.Equal(product.StoreId, result.StoreId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProductDto()
        {
            // Arrange
            var companyId = 1L;
            var storeId = 1L;
            var productId = 1L;
            var product = new Product { Id = productId, Name = "Test Product", Value = 100, StoreId = storeId };

            var store = new StoreDto(storeId, "Test Store", companyId, null);
            _storeServiceMock.Setup(s => s.GetByIdAsync(companyId, storeId)).ReturnsAsync(store);
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(storeId, productId)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetByIdAsync(companyId, storeId, productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Value, result.Value);
            Assert.Equal(product.StoreId, result.StoreId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            // Arrange
            var companyId = 1L;
            var storeId = 1L;
            var productId = 1L;
            var product = new Product { Id = productId, Name = "Old Product", Value = 100, StoreId = storeId };
            var updateDto = new CreateUpdateProductDto("Updated Product", 200);

            var store = new StoreDto(storeId, "Test Store", companyId, null);
            _storeServiceMock.Setup(s => s.GetByIdAsync(companyId, storeId)).ReturnsAsync(store);
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(storeId, productId)).ReturnsAsync(product);

            // Act
            await _productService.UpdateAsync(companyId, storeId, productId, updateDto);

            // Assert
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Product>(p => p.Id == productId && p.Name == updateDto.Name && p.Value == updateDto.Value && p.StoreId == storeId)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProduct()
        {
            // Arrange
            var companyId = 1L;
            var storeId = 1L;
            var productId = 1L;
            var product = new Product { Id = productId, Name = "Test Product", Value = 100, StoreId = storeId };

            var store = new StoreDto(storeId, "Test Store", companyId, null);
            _storeServiceMock.Setup(s => s.GetByIdAsync(companyId, storeId)).ReturnsAsync(store);
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(storeId, productId)).ReturnsAsync(product);

            // Act
            await _productService.DeleteAsync(companyId, storeId, productId);

            // Assert
            _productRepositoryMock.Verify(repo => repo.GetByIdAsync(storeId, productId), Times.Once);
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(product), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfProductDtos()
        {
            // Arrange
            var companyId = 1L;
            var storeId = 1L;
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Value = 100, StoreId = storeId },
                new Product { Id = 2, Name = "Product 2", Value = 200, StoreId = storeId }
            };

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(storeId)).ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllAsync(companyId, storeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(products[0].Id, result.First().Id);
            Assert.Equal(products[1].Id, result.Last().Id);
            Assert.Equal(products[0].Name, result.First().Name);
            Assert.Equal(products[1].Name, result.Last().Name);
            Assert.Equal(products[0].Value, result.First().Value);
            Assert.Equal(products[1].Value, result.Last().Value);
            Assert.Equal(products[0].StoreId, result.First().StoreId);
            Assert.Equal(products[1].StoreId, result.Last().StoreId);
        }
    }
}