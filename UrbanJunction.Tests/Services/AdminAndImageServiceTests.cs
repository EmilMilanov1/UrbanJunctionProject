using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Implementations;
using UrbanJunction.Tests.Helpers;

namespace UrbanJunction.Tests.Services
{
    [TestFixture]
    public class AdminServiceTests
    {
        private Mock<UserManager<UrbanUser>> _userManagerMock = null!;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<UrbanUser>>();
            _userManagerMock = new Mock<UserManager<UrbanUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }

        private AdminService CreateService(UrbanJunction.Data.ApplicationDbContext context)
        {
            return new AdminService(context, _userManagerMock.Object);
        }

        [Test]
        public async Task GetStatsAsync_ReturnsCorrectUserCount()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var stats = await service.GetStatsAsync();

            Assert.That(stats.TotalUsers, Is.EqualTo(2));
        }

        [Test]
        public async Task GetStatsAsync_ReturnsCorrectPostCount()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var stats = await service.GetStatsAsync();

            Assert.That(stats.TotalPosts, Is.EqualTo(1));
        }

        [Test]
        public async Task DeletePostAsync_ReturnsFalse_WhenPostNotFound()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var result = await service.DeletePostAsync(999);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeletePostAsync_ReturnsTrue_AndRemovesPost()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var result = await service.DeletePostAsync(1);

            Assert.That(result, Is.True);
            Assert.That(context.Posts.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task BanUserAsync_ReturnsFalse_WhenUserNotFound()
        {
            var context = TestDbContextFactory.Create();
            _userManagerMock
                .Setup(u => u.FindByIdAsync("nonexistent"))
                .ReturnsAsync((UrbanUser?)null);

            var service = CreateService(context);
            var result = await service.BanUserAsync("nonexistent");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task BanUserAsync_ReturnsTrue_AndSetsLockout()
        {
            var context = TestDbContextFactory.Create();
            var user = TestDataSeeder.CreateUser("user-1");

            _userManagerMock
                .Setup(u => u.FindByIdAsync("user-1"))
                .ReturnsAsync(user);
            _userManagerMock
                .Setup(u => u.SetLockoutEnabledAsync(user, true))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock
                .Setup(u => u.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue))
                .ReturnsAsync(IdentityResult.Success);

            var service = CreateService(context);
            var result = await service.BanUserAsync("user-1");

            Assert.That(result, Is.True);
            _userManagerMock.Verify(u => u.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue), Times.Once);
        }

        [Test]
        public async Task GetAllPostsAsync_ReturnsAllPosts()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var posts = await service.GetAllPostsAsync();

            Assert.That(posts.Count(), Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class ImageServiceTests
    {
        private ImageService CreateService() => new ImageService();

        [Test]
        public void DeleteImage_DoesNothing_WhenPathIsEmpty()
        {
            var service = CreateService();
            Assert.DoesNotThrow(() => service.DeleteImage("", "/wwwroot"));
        }

        [Test]
        public void DeleteImage_DoesNothing_WhenPathIsNull()
        {
            var service = CreateService();
            Assert.DoesNotThrow(() => service.DeleteImage(null!, "/wwwroot"));
        }

        [Test]
        public async Task SaveImagesAsync_ReturnsEmpty_WhenNoFiles()
        {
            var service = CreateService();
            var result = await service.SaveImagesAsync(new List<Microsoft.AspNetCore.Http.IFormFile>(), "/tmp");
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task SaveImagesAsync_SkipsFiles_WhenExceedsSizeLimit()
        {
            var service = CreateService();

            var fileMock = new Mock<Microsoft.AspNetCore.Http.IFormFile>();
            fileMock.Setup(f => f.Length).Returns(6 * 1024 * 1024); // 6MB > 5MB limit
            fileMock.Setup(f => f.FileName).Returns("big.jpg");

            var result = await service.SaveImagesAsync(
                new List<Microsoft.AspNetCore.Http.IFormFile> { fileMock.Object }, "/tmp");

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task SaveImagesAsync_SkipsFiles_WithDisallowedExtension()
        {
            var service = CreateService();

            var fileMock = new Mock<Microsoft.AspNetCore.Http.IFormFile>();
            fileMock.Setup(f => f.Length).Returns(100);
            fileMock.Setup(f => f.FileName).Returns("script.exe");

            var result = await service.SaveImagesAsync(
                new List<Microsoft.AspNetCore.Http.IFormFile> { fileMock.Object }, "/tmp");

            Assert.That(result, Is.Empty);
        }
    }
}
