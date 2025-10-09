using BudgetBay.Data;
using BudgetBay.Models;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace BudgetBay.Test.Repositories
{
    public class UserRepositoryTest
    {
        private readonly Mock<AppDbContext> _mockAppDbContextMock;

        public UserRepositoryTest()
        {
            _mockAppDbContextMock = new Mock<AppDbContext>();
        }

        private DbContextOptions<AppDbContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;
        }
        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            
            var options = GetInMemoryOptions();
            using var context = new AppDbContext(options);
            var repository = new BudgetBay.Repositories.UserRepository(context);
            var user = new User { Username = "testuser", Email = "test@example.com", PasswordHash = "12345" };
            var result = await repository.AddAsync(user);

            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
            Assert.Equal(1, await context.Users.CountAsync());
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_User()
        {
            var options = GetInMemoryOptions();
            using var context = new AppDbContext(options);
            var repository = new BudgetBay.Repositories.UserRepository(context);
            var user = new User { Username = "testuser", Email = "test@example.com", PasswordHash = "12345" };
            var result = await repository.UpdateAsync(user);

            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
            var updatedUser = await context.Users.FindAsync(1);
            Assert.Equal("test@example.com", updatedUser!.Email);
        }
        [Fact]
        public async Task DeleteAsync_Should_Remove_User()
        {
            var options = GetInMemoryOptions();

            using var context = new AppDbContext(options);
            context.Users.Add(new User { Username = "testuser", Email = "test@example.com", PasswordHash = "12345" });
            await context.SaveChangesAsync();

            var repository = new BudgetBay.Repositories.UserRepository(context);

            await repository.DeleteAsync(1);

            var user = await context.Users.FindAsync(1);
            Assert.Null(user);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Users()
        {
            var options = GetInMemoryOptions();

            using var context = new AppDbContext(options);
            context.Users.AddRange(new List<User>
        {
            new User { Username = "testuser", Email = "test@example.com", PasswordHash = "12345" },
            new User { Username = "testuser", Email = "test@example.com", PasswordHash = "12345" }
        });
            await context.SaveChangesAsync();

            var repository = new BudgetBay.Repositories.UserRepository(context);
            var result = await repository.GetAllAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_User_When_Exists()
        {
            var options = GetInMemoryOptions();

            using var context = new AppDbContext(options);
            context.Users.Add(new User { Username = "testuser", Email = "test@example.com", PasswordHash = "12345" });
            await context.SaveChangesAsync();

            var repository = new BudgetBay.Repositories.UserRepository(context);
            var result = await repository.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_User_When_Exists()
        {
            var options = GetInMemoryOptions();

            using var context = new AppDbContext(options);
            context.Users.Add(new User { Username = "testuser", Email = "test@example.com", PasswordHash = "12345" });
            await context.SaveChangesAsync();

            var repository = new BudgetBay.Repositories.UserRepository(context);
            var result = await repository.GetByEmailAsync("test@example.com");

            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task UsernameExistsAsync_Should_Return_True_If_Exists()
        {
            var options = GetInMemoryOptions();

            using var context = new AppDbContext(options);
            context.Users.Add(new User { Username = "testuser", Email = "test@example.com", PasswordHash = "12345" });
            await context.SaveChangesAsync();

            var repository = new BudgetBay.Repositories.UserRepository(context);
            var exists = await repository.UsernameExistsAsync("testuser");

            Assert.True(exists);
        }

        [Fact]
        public async Task EmailExistsAsync_Should_Return_True_If_Exists()
        {
            var options = GetInMemoryOptions();

            using var context = new AppDbContext(options);
            context.Users.Add(new User { Username = "testuser", Email = "test@example.com", PasswordHash = "12345" });
            await context.SaveChangesAsync();

            var repository = new BudgetBay.Repositories.UserRepository(context);
            var exists = await repository.EmailExistsAsync("test@example.com");

            Assert.True(exists);
        }

        [Fact]
        public async Task UserExistsAsync_Should_Return_True_If_User_Exists()
        {
            var options = GetInMemoryOptions();

            using var context = new AppDbContext(options);
            context.Users.Add(new User { Username = "testuser", Email = "test@example.com", PasswordHash = "12345" });
            await context.SaveChangesAsync();

            var repository = new BudgetBay.Repositories.UserRepository(context);
            var exists = await repository.UserExistsAsync(1);

            Assert.True(exists);
        }
    }
}
