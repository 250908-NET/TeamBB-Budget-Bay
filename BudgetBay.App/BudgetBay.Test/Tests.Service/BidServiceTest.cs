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
    public class BidServiceTest
    {
        private readonly Mock<IBidRepository> _mockBidRepo;
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<ILogger<BidService>> _mockLogger;
        private readonly BidService _bidService;
        public BidServiceTest()
        {
            _mockLogger = new Mock<ILogger<BidService>>();
            _mockBidRepo = new Mock<IBidRepository>();
            _mockProductRepo = new Mock<IProductRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _bidService = new BidService(_mockLogger.Object, _mockBidRepo.Object, _mockProductRepo.Object, _mockUserRepo.Object);
        }

        [Fact]
        public async Task GetAllBids_ShouldReturnAllBids()
        {
            // Arrange
            var bids = new List<Bid>
            {
                new Bid { Id = 1, Amount = 100, ProductId = 1, BidderId = 1 },
                new Bid { Id = 2, Amount = 200, ProductId = 2, BidderId = 2 }
            };
            _mockBidRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(bids);

            // Act
            var result = await _bidService.GetAllBids();

            // Assert
            _mockBidRepo.Verify(repo => repo.GetAllAsync(), Times.Once);
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(bids);
        }
        [Fact]
        public async Task CreateBid_ValidBid_ShouldCreateBidIfFirstBid()
        {
            // Arrange
            var newBid = new Bid { Id = 1, Amount = 150, ProductId = 1, BidderId = 2 };
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                ImageUrl = "http://example.com/image.jpg",
                Condition = ProductCondition.New,
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now.AddHours(1),
                StartingPrice = 100,
                CurrentPrice = 100,
                SellerId = 1
            };

            // Act
            _mockBidRepo.Setup(repo => repo.GetByProductIdAsync(newBid.ProductId)).ReturnsAsync(new List<Bid>());
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(newBid.ProductId)).ReturnsAsync(product);
            _mockProductRepo.Setup(repo => repo.UpdateProductAsync(newBid.ProductId, (double)newBid.Amount)).ReturnsAsync(product);
            _mockBidRepo.Setup(repo => repo.AddAsync(newBid)).ReturnsAsync(newBid);

            var createdBid = await _bidService.CreateBid(newBid);
            // Assert
            _mockProductRepo.Verify(repo => repo.UpdateProductAsync(newBid.ProductId, (double)newBid.Amount), Times.Once);
            _mockBidRepo.Verify(repo => repo.AddAsync(newBid), Times.Once);
            createdBid.Should().BeEquivalentTo(newBid);

        }
        [Fact]
        public async Task CreateBid_ValidBid_ShouldCreateBidIfHigherThanCurrentHighestBid()
        {
            // Arrange
            var newBid = new Bid { Id = 1, Amount = 250, ProductId = 1, BidderId = 2 };
            var existingBids = new List<Bid>
            {
                new Bid { Id = 2, Amount = 200, ProductId = 1, BidderId = 3 },
                new Bid { Id = 3, Amount = 150, ProductId = 1, BidderId = 4 }
            };
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                ImageUrl = "http://example.com/image.jpg",
                Condition = ProductCondition.New,
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now.AddHours(1),
                StartingPrice = 100,
                CurrentPrice = 200,
                SellerId = 1
            };
            var user = new User
            {
                Id = 4,
                Username = "seller",
                Email = "seller@example.com",
                PasswordHash = "hashedpassword",
                AddressId = null
            };

            // Act
            _mockBidRepo.Setup(repo => repo.GetByProductIdAsync(newBid.ProductId)).ReturnsAsync(existingBids);
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(newBid.ProductId)).ReturnsAsync(product);
            _mockProductRepo.Setup(repo => repo.UpdateProductAsync(newBid.ProductId, (double)newBid.Amount)).ReturnsAsync(product);
            _mockBidRepo.Setup(repo => repo.AddAsync(newBid)).ReturnsAsync(newBid);
            _mockBidRepo.Setup(repo => repo.GetByUserIdAsync(newBid.BidderId)).ReturnsAsync(new List<Bid> { newBid });

            var createdBid = await _bidService.CreateBid(newBid);
            // Assert
            _mockProductRepo.Verify(repo => repo.UpdateProductAsync(newBid.ProductId, (double)newBid.Amount), Times.Once);
            _mockBidRepo.Verify(repo => repo.AddAsync(newBid), Times.Once);
            createdBid.Should().BeEquivalentTo(newBid);
        }
        [Fact]
        public async Task CreateBid_InvalidBid_ShouldNotCreateBidIfLowerThanCurrentHighestBid()
        {
            // Arrange
            var newBid = new Bid { Id = 1, Amount = 150, ProductId = 1, BidderId = 2 };
            var existingBids = new List<Bid>
            {
                new Bid { Id = 2, Amount = 200, ProductId = 1, BidderId = 3 },
                new Bid { Id = 3, Amount = 180, ProductId = 1, BidderId = 4 }
            };
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                ImageUrl = "http://example.com/image.jpg",
                Condition = ProductCondition.New,
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now.AddHours(1),
                StartingPrice = 100,
                CurrentPrice = 200,
                SellerId = 1
            };
            var user = new User
            {
                Id = 4,
                Username = "seller",
                Email = "seller@example.com",
                PasswordHash = "hashedpassword",
                AddressId = null
            };

            // Act
            _mockBidRepo.Setup(repo => repo.GetByProductIdAsync(newBid.ProductId)).ReturnsAsync(existingBids);
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(newBid.ProductId)).ReturnsAsync(product);
            _mockBidRepo.Setup(repo => repo.GetByUserIdAsync(newBid.BidderId)).ReturnsAsync(new List<Bid> { newBid });


            var createdBid = await _bidService.CreateBid(newBid);
            // Assert
            _mockProductRepo.Verify(repo => repo.UpdateProductAsync(It.IsAny<int>(), It.IsAny<double>()), Times.Never);
            _mockBidRepo.Verify(repo => repo.AddAsync(It.IsAny<Bid>()), Times.Never);
            createdBid.Should().BeNull();
        }
        [Fact]
        public async Task CreateBid_InvalidBid_ShouldNotCreateBidIfBidderIsSeller()
        {
            // Arrange
            var newBid = new Bid { Id = 1, Amount = 150, ProductId = 1, BidderId = 1 }; // BidderId is same as SellerId
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Description = "Test Description",
                ImageUrl = "http://example.com/image.jpg",
                Condition = ProductCondition.New,
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now.AddHours(1),
                StartingPrice = 100,
                CurrentPrice = 100,
                SellerId = 1
            };
            var user = new User
            {
                Id = 1,
                Username = "seller",
                Email = "seller@example.com",
                PasswordHash = "hashedpassword",
                AddressId = null
            };


            // Act
            _mockBidRepo.Setup(repo => repo.GetByProductIdAsync(newBid.ProductId)).ReturnsAsync(new List<Bid>());
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(newBid.ProductId)).ReturnsAsync(product);
            _mockBidRepo.Setup(repo => repo.GetByUserIdAsync(newBid.BidderId)).ReturnsAsync(new List<Bid> { newBid });

            var createdBid = await _bidService.CreateBid(newBid);
            // Assert
            _mockProductRepo.Verify(repo => repo.UpdateProductAsync(It.IsAny<int>(), It.IsAny<double>()), Times.Never);
            _mockBidRepo.Verify(repo => repo.AddAsync(It.IsAny<Bid>()), Times.Never);
            createdBid.Should().BeNull();
        }

        [Fact]
        public async Task CancelBid_ExistingBid_ShouldCancelBids()
        {
            // Arrange
            int productId = 1;
            int userId = 2;
            var bids = new List<Bid>
            {
                new Bid { Id = 1, Amount = 150, ProductId = productId, BidderId = userId },
                new Bid { Id = 2, Amount = 200, ProductId = productId, BidderId = 3 },
                new Bid { Id = 3, Amount = 180, ProductId = productId, BidderId = 4 },
                new Bid { Id = 4, Amount = 220, ProductId = productId, BidderId = userId },
                new Bid { Id = 5, Amount = 300, ProductId = 2, BidderId = userId }
            };


            // Act
            _mockBidRepo.Setup(repo => repo.GetByProductIdAsync(productId)).ReturnsAsync(bids.Where(b => b.ProductId == productId).ToList());
            _mockBidRepo.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(bids.Where(b => b.BidderId == userId).ToList());


            await _bidService.CancelBid(productId, userId);
            // Assert
            _mockBidRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Bid>()), Times.Exactly(2));
            _mockProductRepo.Verify(repo => repo.UpdateProductAsync(productId, It.IsAny<double>()), Times.Once); // once for each remaining bid
        }
        [Fact]
        public async Task CancelBid_NonExistingBid_ShouldNotCancelBids()
        {
            // Arrange
            int productId = 1;
            int userId = 2;
            var bids = new List<Bid>
            {
                new Bid { Id = 2, Amount = 200, ProductId = productId, BidderId = 3 },
                new Bid { Id = 3, Amount = 180, ProductId = productId, BidderId = 4 },
                new Bid { Id = 5, Amount = 300, ProductId = 2, BidderId = userId }
            };


            // Act
            _mockBidRepo.Setup(repo => repo.GetByProductIdAsync(productId)).ReturnsAsync(bids.Where(b => b.ProductId == productId).ToList());
            _mockBidRepo.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(bids.Where(b => b.BidderId == userId).ToList());


            await _bidService.CancelBid(productId, userId);
            // Assert
            _mockBidRepo.Verify(repo => repo.DeleteAsync(It.IsAny<Bid>()), Times.Never);
            _mockProductRepo.Verify(repo => repo.UpdateProductAsync(productId, It.IsAny<double>()), Times.Never); // once for each remaining bid
        }
        [Fact]
        public async Task GetHighestBid_ExistingBids_ShouldReturnHighestBid()
        {
            // Arrange
            int productId = 1;
            var bids = new List<Bid>
            {
                new Bid { Id = 1, Amount = 150, ProductId = productId, BidderId = 2 },
                new Bid { Id = 2, Amount = 200, ProductId = productId, BidderId = 3 },
                new Bid { Id = 3, Amount = 180, ProductId = 2, BidderId = 4 }
            };
            _mockBidRepo.Setup(repo => repo.GetByProductIdAsync(productId)).ReturnsAsync(bids);

            // Act
            var highestBid = await _bidService.GetHighestBid(productId);

            // Assert
            _mockBidRepo.Verify(repo => repo.GetByProductIdAsync(productId), Times.Once);
            highestBid.Should().Be(200);
        }
        [Fact]
        public async Task GetHighestBid_NoBids_ShouldReturnNull()
        {
            // Arrange
            int productId = 1;
            var bids = new List<Bid>
            {
                new Bid { Id = 1, Amount = 150, ProductId = 4, BidderId = 2 },
                new Bid { Id = 2, Amount = 200, ProductId = 3, BidderId = 3 },
                new Bid { Id = 3, Amount = 180, ProductId = 2, BidderId = 4 }
            };
            _mockBidRepo.Setup(repo => repo.GetByProductIdAsync(productId)).ReturnsAsync(new List<Bid>());

            // Act
            var highestBid = await _bidService.GetHighestBid(productId);

            // Assert
            _mockBidRepo.Verify(repo => repo.GetByProductIdAsync(productId), Times.Once);
            highestBid.Should().BeNull();
        }
        [Fact]
        public async Task GetBidsByProductId_ExistingBids_ShouldReturnBids()
        {
            // Arrange
            int productId = 1;
            var bids = new List<Bid>
            {
                new Bid { Id = 1, Amount = 150, ProductId = 2, BidderId = 2 },
                new Bid { Id = 2, Amount = 200, ProductId = productId, BidderId = 3 },
                new Bid { Id = 3, Amount = 180, ProductId = productId, BidderId = 4 }
            };
            _mockBidRepo.Setup(repo => repo.GetByProductIdAsync(productId)).ReturnsAsync(bids.Where(b => b.ProductId == productId).ToList());

            // Act
            var resultBids = await _bidService.GetBidsByProductId(productId);

            // Assert
            _mockBidRepo.Verify(repo => repo.GetByProductIdAsync(productId), Times.Once);
            resultBids.Should().HaveCount(2);
            resultBids.Should().BeEquivalentTo(bids.Where(b => b.ProductId == productId));
        }
        [Fact]
        public async Task GetBidsByProductId_NoBids_ShouldReturnNull()
        {
            // Arrange
            int productId = 1;
            var bids = new List<Bid>
            {
                new Bid { Id = 1, Amount = 150, ProductId = 2, BidderId = 2 },
                new Bid { Id = 2, Amount = 200, ProductId = 3, BidderId = 3 },
                new Bid { Id = 3, Amount = 180, ProductId = 4, BidderId = 4 }
            };
            _mockBidRepo.Setup(repo => repo.GetByProductIdAsync(productId)).ReturnsAsync(new List<Bid>());

            // Act
            var resultBids = await _bidService.GetBidsByProductId(productId);

            // Assert
            _mockBidRepo.Verify(repo => repo.GetByProductIdAsync(productId), Times.Once);
            resultBids.Should().BeNull();
        }
        [Fact]
        public async Task GetBidsByUserId_ExistingBids_ShouldReturnBids()
        {
            // Arrange
            int userId = 2;
            var bids = new List<Bid>
            {
                new Bid { Id = 1, Amount = 150, ProductId = 1, BidderId = userId },
                new Bid { Id = 2, Amount = 200, ProductId = 2, BidderId = userId },
                new Bid { Id = 3, Amount = 180, ProductId = 3, BidderId = userId }
            };
            _mockBidRepo.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(bids);

            // Act
            var resultBids = await _bidService.GetBidsByUserId(userId);

            // Assert
            _mockBidRepo.Verify(repo => repo.GetByUserIdAsync(userId), Times.Once);
            resultBids.Should().HaveCount(3);
            resultBids.Should().BeEquivalentTo(bids);
        }
        [Fact]
        public async Task GetBidsByUserId_NoBids_ShouldReturnNull()
        {
            // Arrange
            int userId = 2;
            _mockBidRepo.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(new List<Bid>());

            // Act
            var resultBids = await _bidService.GetBidsByUserId(userId);

            // Assert
            _mockBidRepo.Verify(repo => repo.GetByUserIdAsync(userId), Times.Once);
            resultBids.Should().BeNull();
        }

    }

}