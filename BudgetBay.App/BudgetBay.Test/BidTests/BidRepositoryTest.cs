using Xunit;
using FluentAssertions;
using Moq;
using BudgetBay.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using BudgetBay.Services;
using BudgetBay.Models;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
namespace BudgetBay.Test.BidTests
{
    public class BidTest : TestBase
    {
        private readonly BidRepository _repository;
        public BidTest()
        {
            _repository = new BidRepository(Context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBids()
        {
            // Arrange
            await SeedDataAsync();

            // Act
            var bids = await _repository.GetAllAsync();
            // Assert
            bids.Should().NotBeNull();
            bids.Should().HaveCount(4);

            int[] expectedBidIds = { 1, 2, 3, 4 };
            decimal[] expectedBidAmounts = { 50.00m, 75.00m, 100.00m, 150.00m };
            int[] expectedBidderIds = { 1, 2, 3, 1 };
            int[] expectedProductIds = { 1, 1, 2, 2 };

            for (int i = 0; i < bids.Count; i++)
            {
                bids[i].Id.Should().Be(expectedBidIds[i]);
                bids[i].Amount.Should().Be(expectedBidAmounts[i]);
                bids[i].BidderId.Should().Be(expectedBidderIds[i]);
                bids[i].ProductId.Should().Be(expectedProductIds[i]);

            }


        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectBid()
        {
            //Arrange
            await SeedDataAsync();

            //Act
            var bid = await _repository.GetByIdAsync(2);

            //Assert
            bid.Should().NotBeNull();
            bid!.Id.Should().Be(2);
            bid.Amount.Should().Be(75.00m);
            bid.BidderId.Should().Be(2);
            bid.ProductId.Should().Be(1);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullForNonExistentId()
        {
            //Arrange
            await SeedDataAsync();

            //Act
            var bid = await _repository.GetByIdAsync(999);
            //Assert
            bid.Should().BeNull();
        }
        [Fact]
        public async Task AddAsync_ShouldAddNewBid()
        {
            //Arrange
            await SeedDataAsync();

            var newBid = new Bid
            {
                Amount = 200.00m,
                BidderId = 2,
                ProductId = 3,
                CreatedAt = DateTime.UtcNow
            };
            //Act
            var addedBid = await _repository.AddAsync(newBid);
            //Assert
            addedBid.Should().NotBeNull();
            addedBid.Id.Should().BeGreaterThan(0);
            addedBid.Amount.Should().Be(200.00m);
            addedBid.BidderId.Should().Be(2);
            addedBid.ProductId.Should().Be(3);

            var bids = await _repository.GetAllAsync();
            bids.Should().HaveCount(5);
        }

        [Fact]
        public async Task GetByProductIdAsync_ShouldReturnBidsForProduct()
        {
            //Arrange
            await SeedDataAsync();

            //Act
            var bids = await _repository.GetByProductIdAsync(1);
            //Assert
            bids.Should().NotBeNull();
            bids.Should().HaveCount(2);

            foreach (var bid in bids)
            {
                bid.ProductId.Should().Be(1);
            }
        }
        [Fact]
        public async Task GetByProductIdAsync_ShouldReturnEmptyForProductWithNoBids()
        {
            //Arrange
            await SeedDataAsync();

            //Act
            var bids = await _repository.GetByProductIdAsync(3);
            //Assert
            bids.Should().NotBeNull();
            bids.Should().BeEmpty();
        }
        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnBidsForUser()
        {
            //Arrange
            await SeedDataAsync();

            //Act
            var bids = await _repository.GetByUserIdAsync(1);
            //Assert
            bids.Should().NotBeNull();
            bids.Should().HaveCount(2);

            foreach (var bid in bids)
            {
                bid.BidderId.Should().Be(1);
            }
        }
        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnEmptyForUserWithNoBids()
        {
            //Arrange
            await SeedDataAsync();

            //Act
            var bids = await _repository.GetByUserIdAsync(4);
            //Assert
            bids.Should().NotBeNull();
            bids.Should().BeEmpty();
        }
        [Fact]
        public async Task DeleteAsync_ShouldRemoveBid()
        {
            //Arrange
            await SeedDataAsync();

            var bid = await _repository.GetByIdAsync(3);
            bid.Should().NotBeNull();

            //Act
            await _repository.DeleteAsync(bid!);

            //Assert
            var deletedBid = await _repository.GetByIdAsync(3);
            deletedBid.Should().BeNull();

            var bids = await _repository.GetAllAsync();
            bids.Should().HaveCount(3);
        }
        public async Task DeleteAsync_ShouldHandleNonExistent()
        {
            //Arrange
            await SeedDataAsync();

            var nonExistentBid = new Bid { Id = 999, Amount = 0, BidderId = 0, ProductId = 0 };

            //Act
            Func<Task> act = async () => await _repository.DeleteAsync(nonExistentBid);

            //Assert

            await act.Should().NotThrowAsync();

            var bids = await _repository.GetAllAsync();
            bids.Should().HaveCount(4);
        }
    }

        
}