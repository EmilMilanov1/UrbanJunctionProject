using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Hosting;
using UrbanJunction.Data.Models;
using UrbanJunction.Data.ViewModels;
using UrbanJunction.Services.Implementations;
using UrbanJunction.Services.Interfaces;
using UrbanJunction.Tests.Helpers;

namespace UrbanJunction.Tests.Services
{
    [TestFixture]
    public class PostServiceTests
    {
        private Mock<IImageService> _imageServiceMock = null!;
        private Mock<IWebHostEnvironment> _envMock = null!;

        [SetUp]
        public void SetUp()
        {
            _imageServiceMock = new Mock<IImageService>();
            _envMock = new Mock<IWebHostEnvironment>();
            _envMock.Setup(e => e.WebRootPath).Returns("/wwwroot");
        }

        private PostService CreateService(UrbanJunction.Data.ApplicationDbContext context)
        {
            return new PostService(context, _imageServiceMock.Object, _envMock.Object);
        }

        [Test]
        public async Task GetByTopicAsync_ReturnsOnlyPostsMatchingTopic()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var musicTopic = TestDataSeeder.CreateTopic(2, "Music");
            var musicSubcat = TestDataSeeder.CreateSubcategory(2, "Techno", 2);
            var musicPost = TestDataSeeder.CreatePost(2, "Music Post", "Music content", "user-1", 2);
            context.Topics.Add(musicTopic);
            context.Subcategories.Add(musicSubcat);
            context.Posts.Add(musicPost);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var results = await service.GetByTopicAsync("Art");

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Title, Is.EqualTo("Test Post"));
        }

        [Test]
        public async Task GetByTopicAsync_WithSubcat_FiltersCorrectly()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var subcat2 = TestDataSeeder.CreateSubcategory(2, "Photography", 1);
            var post2 = TestDataSeeder.CreatePost(2, "Photography Post", "Photo content", "user-1", 2);
            context.Subcategories.Add(subcat2);
            context.Posts.Add(post2);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var results = await service.GetByTopicAsync("Art", "Photography");

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Title, Is.EqualTo("Photography Post"));
        }

        [Test]
        public async Task SearchAsync_ReturnsPostsMatchingQuery()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var results = await service.SearchAsync("Art", "Test");

            Assert.That(results.Any(), Is.True);
            Assert.That(results.First().Title, Is.EqualTo("Test Post"));
        }

        [Test]
        public async Task SearchAsync_ReturnsEmpty_WhenNoMatch()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var results = await service.SearchAsync("Art", "xxxxnotfound");

            Assert.That(results.Any(), Is.False);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsNull_WhenPostNotFound()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var result = await service.GetDetailsAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetDetailsAsync_ReturnsPost_WhenFound()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var result = await service.GetDetailsAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Title, Is.EqualTo("Test Post"));
        }

        [Test]
        public async Task CreateAsync_SavesPostAndReturnsIt()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            _imageServiceMock
                .Setup(s => s.SaveImagesAsync(It.IsAny<List<Microsoft.AspNetCore.Http.IFormFile>>(), It.IsAny<string>()))
                .ReturnsAsync(new List<string>());

            var service = CreateService(context);
            var model = new PostFormViewModel
            {
                Title = "New Post",
                Content = "New content",
                SubcategoryId = 1
            };

            var result = await service.CreateAsync(model, "user-1");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo("New Post"));
            Assert.That(context.Posts.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task EditAsync_ReturnsFalse_WhenPostNotFound()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var model = new PostFormViewModel { Title = "Updated", Content = "Updated", SubcategoryId = 1 };
            var result = await service.EditAsync(999, model, "user-1");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task EditAsync_ReturnsFalse_WhenUserIsNotOwner()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var model = new PostFormViewModel { Title = "Updated", Content = "Updated", SubcategoryId = 1 };
            var result = await service.EditAsync(1, model, "user-2");

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task EditAsync_ReturnsTrue_WhenOwnerEdits()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var model = new PostFormViewModel { Title = "Updated Title", Content = "Updated content", SubcategoryId = 1 };
            var result = await service.EditAsync(1, model, "user-1");

            Assert.That(result, Is.True);
            Assert.That(context.Posts.First().Title, Is.EqualTo("Updated Title"));
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenPostNotFound()
        {
            var context = TestDbContextFactory.Create();
            var service = CreateService(context);

            var result = await service.DeleteAsync(999, "user-1", false);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenNotOwnerAndNotAdmin()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var result = await service.DeleteAsync(1, "user-2", false);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_ReturnsTrue_WhenAdmin()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var result = await service.DeleteAsync(1, "user-2", isAdmin: true);

            Assert.That(result, Is.True);
            Assert.That(context.Posts.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GetByUserAsync_ReturnsOnlyUserPosts()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post2 = TestDataSeeder.CreatePost(2, "User2 Post", "content", "user-2", 1);
            context.Posts.Add(post2);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var results = await service.GetByUserAsync("user-1");

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().UserId, Is.EqualTo("user-1"));
        }

        [Test]
        public async Task SearchAllAsync_FiltersbyTopic()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var musicTopic = TestDataSeeder.CreateTopic(2, "Music");
            var musicSubcat = TestDataSeeder.CreateSubcategory(2, "Techno", 2);
            var musicPost = TestDataSeeder.CreatePost(2, "Music Post", "content", "user-1", 2);
            context.Topics.Add(musicTopic);
            context.Subcategories.Add(musicSubcat);
            context.Posts.Add(musicPost);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var results = await service.SearchAllAsync(null, "Music", "new");

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().Title, Is.EqualTo("Music Post"));
        }
    }
}
