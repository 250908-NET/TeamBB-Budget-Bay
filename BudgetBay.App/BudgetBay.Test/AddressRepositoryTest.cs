using Xunit;
using BudgetBay.Repositories;
using BudgetBay.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BudgetBay.Test
{
    public class AddressRepositoryTests : TestBase
    {
        private readonly AddressRepository _repository;

        public AddressRepositoryTests()
        {
            _repository = new AddressRepository(Context);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllAddresses()
        {
            // Arrange
            await SeedDataAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectAddress()
        {
            // Arrange
            await SeedDataAsync();

            // Act
            var result = await _repository.GetByIdAsync(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Elm St", result.StreetName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange
            await SeedDataAsync();

            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsNewAddress()
        {
            // Arrange
            await SeedDataAsync();
            var newAddress = new Address
            {
                StreetNumber = "101",
                StreetName = "Pine St",
                City = "Springfield",
                State = "IL",
                ZipCode = "62704"
            };

            // Act
            var result = await _repository.AddAsync(newAddress);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);

            var allAddresses = await _repository.GetAllAsync();
            Assert.Equal(4, allAddresses.Count);
        }

        [Fact]
        public async Task UpdateAsync_UpdateExistingAddress()
        {
            // Arrange
            await SeedDataAsync();
            var address = await _repository.GetByIdAsync(1);
            address.StreetName = "Updated St";

            // Act
            var result = await _repository.UpdateAsync(address);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated St", result.StreetName);

            var DbAddres = await _repository.GetByIdAsync(1);
            Assert.Equal("Updated St", DbAddres.StreetName);
        }

        [Fact]
        public async Task DeleteAsync_RemoveAddress()
        {
            // Arrange
            await SeedDataAsync();

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var result = await _repository.GetByIdAsync(1);
            Assert.Null(result);

            var allAddresses = await _repository.GetAllAsync();
            Assert.Equal(2, allAddresses.Count);
        }
    }
}