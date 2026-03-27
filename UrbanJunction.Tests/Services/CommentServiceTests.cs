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

            var post = context.Posts.First();
            var service = CreateService(context);
            await service.AddAsync(post.Id, "Great post!", "user-2");

            Assert.That(context.Comments.Count(), Is.EqualTo(1));
            Assert.That(context.Comments.First().Content, Is.EqualTo("Great post!"));

            _notifMock.Verify(n => n.CreateAsync(
                "user-1", "user-2", NotificationType.Comment,
                post.Id, It.IsAny<int>(), null), Times.Once);
        }

        [Test]
        public async Task AddAsync_DoesNotNotify_WhenCommenterIsPostOwner()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var service = CreateService(context);
            await service.AddAsync(post.Id, "My own post comment", "user-1");

            _notifMock.Verify(n => n.CreateAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NotificationType>(),
                It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Never);
        }

        [Test]
        public async Task AddAsync_WithParentCommentId_NotifiesParentCommentAuthor()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var parentComment = TestDataSeeder.CreateComment("Parent comment", "user-2", post.Id);
            context.Comments.Add(parentComment);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            await service.AddAsync(post.Id, "Reply here", "user-1", parentCommentId: parentComment.Id);

            _notifMock.Verify(n => n.CreateAsync(
                "user-2", "user-1", NotificationType.Reply,
                post.Id, It.IsAny<int>(), null), Times.Once);
        }

        [Test]
        public async Task AddAsync_WithParentComment_DoesNotNotify_WhenReplierIsParentAuthor()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var parentComment = TestDataSeeder.CreateComment("Parent comment", "user-2", post.Id);
            context.Comments.Add(parentComment);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            await service.AddAsync(post.Id, "Reply to myself", "user-2", parentCommentId: parentComment.Id);

            _notifMock.Verify(n => n.CreateAsync(
                It.IsAny<string>(), It.IsAny<string>(), NotificationType.Reply,
                It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Never);
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

            var post = context.Posts.First();
            var comment = TestDataSeeder.CreateComment("Some comment", "user-2", post.Id);
            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var result = await service.DeleteAsync(comment.Id, "user-1", false);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_ReturnsTrue_WhenOwnerDeletes()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var comment = TestDataSeeder.CreateComment("Some comment", "user-2", post.Id);
            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var result = await service.DeleteAsync(comment.Id, "user-2", false);

            Assert.That(result, Is.True);
            Assert.That(context.Comments.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task DeleteAsync_ReturnsTrue_WhenAdminDeletes()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var comment = TestDataSeeder.CreateComment("Some comment", "user-2", post.Id);
            context.Comments.Add(comment);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var result = await service.DeleteAsync(comment.Id, "user-1", isAdmin: true);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetByPostAsync_ReturnsOnlyCommentsForPost()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post1 = context.Posts.First();

            var musicTopic = TestDataSeeder.CreateTopic("Music");
            context.Topics.Add(musicTopic);
            await context.SaveChangesAsync();

            var musicSubcat = TestDataSeeder.CreateSubcategory("Techno", musicTopic.Id);
            context.Subcategories.Add(musicSubcat);
            await context.SaveChangesAsync();

            var post2 = TestDataSeeder.CreatePost("Post 2", "Content", "user-1", musicSubcat.Id);
            context.Posts.Add(post2);
            await context.SaveChangesAsync();

            var c1 = TestDataSeeder.CreateComment("Comment on post 1", "user-2", post1.Id);
            var c2 = TestDataSeeder.CreateComment("Comment on post 2", "user-2", post2.Id);
            context.Comments.AddRange(c1, c2);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var results = await service.GetByPostAsync(post1.Id);

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().PostId, Is.EqualTo(post1.Id));
        }
    }
}