using Moq;
using NUnit.Framework;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Implementations;
using UrbanJunction.Tests.Helpers;

namespace UrbanJunction.Tests.Services
{
    [TestFixture]
    public class CommentServiceTests
    {
        private Mock<INotificationService> _notifMock = null!;

        [SetUp]
        public void SetUp()
        {
            _notifMock = new Mock<INotificationService>();
        }

        private CommentService CreateService(UrbanJunction.Data.ApplicationDbContext context)
        {
            return new CommentService(context, _notifMock.Object);
        }

        [Test]
        public async Task AddAsync_CreatesTopLevelComment_AndNotifiesPostOwner()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            await service.AddAsync(1, "Great post!", "user-2");

            Assert.That(context.Comments.Count(), Is.EqualTo(1));
            Assert.That(context.Comments.First().Content, Is.EqualTo("Great post!"));

            _notifMock.Verify(n => n.CreateAsync(
                "user-1",
                "user-2",
                NotificationType.Comment,
                1,
                It.IsAny<int>(),
                null), Times.Once);
        }

        [Test]
        public async Task AddAsync_DoesNotNotify_WhenCommenterIsPostOwner()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            await service.AddAsync(1, "My own post comment", "user-1");

            _notifMock.Verify(n => n.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<NotificationType>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()), Times.Never);
        }

        [Test]
        public async Task AddAsync_WithParentCommentId_NotifiesParentCommentAuthor()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var parentComment = TestDataSeeder.CreateComment(1, "Parent comment", "user-2", 1);
            context.Comments.Add(parentComment);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            await service.AddAsync(1, "Reply here", "user-1", parentCommentId: 1);

            _notifMock.Verify(n => n.CreateAsync(
                "user-2",
                "user-1",
                NotificationType.Reply,
                1,
                It.IsAny<int>(),
                null), Times.Once);
        }

        [Test]
        public async Task AddAsync_WithParentComment_DoesNotNotify_WhenReplierIsParentAuthor()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var parentComment = TestDataSeeder.CreateComment(1, "Parent comment", "user-2", 1);
            context.Comments.Add(parentComment);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            // user-2 replying to their own comment
            await service.AddAsync(1, "Reply to myself", "user-2", parentCommentId: 1);

            _notifMock.Verify(n => n.CreateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                NotificationType.Reply,
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()), Times.Never);
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WhenCommentNotFound()
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

            var comment = TestDataSeeder.CreateComment(1, "Some comment", "user-2", 1);
            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var result = await service.DeleteAsync(1, "user-1", false);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_ReturnsTrue_WhenOwnerDeletes()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var comment = TestDataSeeder.CreateComment(1, "Some comment", "user-2", 1);
            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var result = await service.DeleteAsync(1, "user-2", false);

            Assert.That(result, Is.True);
            Assert.That(context.Comments.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteAsync_ReturnsTrue_WhenAdminDeletes()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var comment = TestDataSeeder.CreateComment(1, "Some comment", "user-2", 1);
            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var result = await service.DeleteAsync(1, "user-1", isAdmin: true);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetByPostAsync_ReturnsOnlyCommentsForPost()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var musicTopic = TestDataSeeder.CreateTopic(2, "Music");
            var musicSubcat = TestDataSeeder.CreateSubcategory(2, "Techno", 2);
            var post2 = TestDataSeeder.CreatePost(2, "Post 2", "Content", "user-1", 2);
            context.Topics.Add(musicTopic);
            context.Subcategories.Add(musicSubcat);
            context.Posts.Add(post2);

            var c1 = TestDataSeeder.CreateComment(1, "Comment on post 1", "user-2", 1);
            var c2 = TestDataSeeder.CreateComment(2, "Comment on post 2", "user-2", 2);
            context.Comments.AddRange(c1, c2);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var results = await service.GetByPostAsync(1);

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().PostId, Is.EqualTo(1));
        }
    }
}
