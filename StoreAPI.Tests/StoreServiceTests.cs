using Moq;
using StoreAPI.Dtos;
using StoreAPI.Models;
using StoreAPI.Repositories;
using StoreAPI.Services;

namespace StoreAPI.Tests
{
    public class StoreServiceTests
    {
        private readonly Mock<IStoreRepository> _storeRepositoryMock;
        private readonly StoreService _storeService;

        public StoreServiceTests()
        {
            _storeRepositoryMock = new Mock<IStoreRepository>();
            _storeService = new StoreService(_storeRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedStoreDto()
        {
            // Arrange
            var companyId = 1L;
            var storeDto = new CreateUpdateStoreDto("Test Store");
            var store = new Store { Id = 1, Name = "Test Store", CompanyId = companyId };

            _storeRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Store>())).ReturnsAsync(store);

            // Act
            var result = await _storeService.CreateAsync(companyId, storeDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(store.Id, result.Id);
            Assert.Equal(store.Name, result.Name);
            Assert.Equal(store.CompanyId, result.CompanyId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnStoreDto()
        {
            // Arrange
            var companyId = 1L;
            var store = new Store
            {
                Id = 1,
                Name = "Test Store",
                CompanyId = companyId,
                Products = new List<Product> { new Product { Id = 1, Name = "Product 1", Value = 100, StoreId = 1 } }
            };

            _storeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<long>(), It.IsAny<long>())).ReturnsAsync(store);

            // Act
            var result = await _storeService.GetByIdAsync(companyId, store.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(store.Id, result.Id);
            Assert.Equal(store.Name, result.Name);
            Assert.Equal(store.CompanyId, result.CompanyId);
            Assert.NotNull(result.Products);
            Assert.Single(result.Products);
            Assert.Equal(store.Products.First().Id, result.Products.First().Id);
            Assert.Equal(store.Products.First().Name, result.Products.First().Name);
            Assert.Equal(store.Products.First().Value, result.Products.First().Value);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateStore()
        {
            // Arrange
            var companyId = 1L;
            var store = new Store { Id = 1, Name = "Old Store Name", CompanyId = companyId };
            var updateDto = new CreateUpdateStoreDto("Updated Store Name");

            _storeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<long>(), It.IsAny<long>())).ReturnsAsync(store);

            // Act
            await _storeService.UpdateAsync(companyId, store.Id, updateDto);

            // Assert
            _storeRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Store>(s => s.Id == store.Id && s.Name == updateDto.Name && s.CompanyId == companyId)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteStore()
        {
            // Arrange
            var companyId = 1L;
            var store = new Store { Id = 1, Name = "Test Store", CompanyId = companyId };

            _storeRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<long>(), It.IsAny<long>())).ReturnsAsync(store);

            // Act
            await _storeService.DeleteAsync(companyId, store.Id);

            // Assert
            _storeRepositoryMock.Verify(repo => repo.DeleteAsync(store), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfStoreDtos()
        {
            // Arrange
            var companyId = 1L;
            var stores = new List<Store>
            {
                new Store { Id = 1, Name = "Store 1", CompanyId = companyId },
                new Store { Id = 2, Name = "Store 2", CompanyId = companyId }
            };

            _storeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<long>())).ReturnsAsync(stores);

            // Act
            var result = await _storeService.GetAllAsync(companyId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(stores[0].Id, result.First().Id);
            Assert.Equal(stores[1].Id, result.Last().Id);
            Assert.Equal(stores[0].Name, result.First().Name);
            Assert.Equal(stores[1].Name, result.Last().Name);
            Assert.Equal(stores[0].CompanyId, result.First().CompanyId);
            Assert.Equal(stores[1].CompanyId, result.Last().CompanyId);
        }
    }
}