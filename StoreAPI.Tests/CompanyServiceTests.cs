using Moq;
using StoreAPI.Dtos;
using StoreAPI.Models;
using StoreAPI.Repositories;
using StoreAPI.Services;

namespace StoreAPI.Tests
{
    public class CompanyServiceTests
    {
        private readonly Mock<ICompanyRepository> _companyRepositoryMock;
        private readonly CompanyService _companyService;

        public CompanyServiceTests()
        {
            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _companyService = new CompanyService(_companyRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedCompanyDto()
        {
            // Arrange
            var companyDto = new CreateUpdateCompanyDto("Test Company", "Test Owner");
            var company = new Company { Id = 1, Name = companyDto.Name, Owner = companyDto.Owner };

            _companyRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Company>())).ReturnsAsync(company);

            // Act
            var result = await _companyService.CreateAsync(companyDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(company.Id, result.Id);
            Assert.Equal(company.Name, result.Name);
            Assert.Equal(company.Owner, result.Owner);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCompanyDto()
        {
            // Arrange
            var company = new Company
            {
                Id = 1,
                Name = "Test Company",
                Owner = "Test Owner",
                Stores = new List<Store> { new Store { Id = 1, Name = "Store 1", CompanyId = 1 } }
            };

            _companyRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(company);

            // Act
            var result = await _companyService.GetByIdAsync(company.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(company.Id, result.Id);
            Assert.Equal(company.Name, result.Name);
            Assert.Equal(company.Owner, result.Owner);
            Assert.NotNull(result.Stores);
            Assert.Single(result.Stores);
            Assert.Equal(company.Stores.First().Id, result.Stores.First().Id);
            Assert.Equal(company.Stores.First().Name, result.Stores.First().Name);
            Assert.Equal(company.Stores.First().CompanyId, result.Stores.First().CompanyId);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateCompany()
        {
            // Arrange
            var company = new Company { Id = 1, Name = "Test Company", Owner = "Test Owner" };
            var updateDto = new CreateUpdateCompanyDto("Updated Company", "Updated Owner");

            _companyRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(company);

            // Act
            await _companyService.UpdateAsync(company.Id, updateDto);

            // Assert
            _companyRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Company>(c => c.Id == company.Id && c.Name == updateDto.Name && c.Owner == updateDto.Owner)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCompany()
        {
            // Arrange
            var company = new Company { Id = 1, Name = "Test Company", Owner = "Test Owner" };

            _companyRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(company);

            // Act
            await _companyService.DeleteAsync(company.Id);

            // Assert
            _companyRepositoryMock.Verify(repo => repo.DeleteAsync(It.Is<Company>(c => c.Id == company.Id)), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfCompanyDtos()
        {
            // Arrange
            var companies = new List<Company>
            {
                new Company { Id = 1, Name = "Test Company 1", Owner = "Test Owner 1" },
                new Company { Id = 2, Name = "Test Company 2", Owner = "Test Owner 2" }
            };

            _companyRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(companies);

            // Act
            var result = await _companyService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(companies[0].Id, result.First().Id);
            Assert.Equal(companies[1].Id, result.Last().Id);
            Assert.Equal(companies[0].Name, result.First().Name);
            Assert.Equal(companies[1].Name, result.Last().Name);
            Assert.Equal(companies[0].Owner, result.First().Owner);
            Assert.Equal(companies[1].Owner, result.Last().Owner);
        }
    }
}