using Moq;
using NUnit.Framework;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Implementations;
using UrbanJunction.Tests.Helpers;

namespace UrbanJunction.Tests.Services
{
    [TestFixture]
    public class ReactionServiceTests
    {
        private Mock<INotificationService> _notifMock = null!;

        [SetUp]
        public void SetUp()
        {
            _notifMock = new Mock<INotificationService>();
        }

        private ReactionService CreateService(UrbanJunction.Data.ApplicationDbContext context)
        {
            return new ReactionService(context, _notifMock.Object);
        }

        [Test]
        public async Task VoteAsync_CreatesNewReaction_WhenNoneExists()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var service = CreateService(context);
            await service.VoteAsync(post.Id, "user-2", isUpvote: true);

            Assert.That(context.Reactions.Count(), Is.EqualTo(1));
            Assert.That(context.Reactions.First().IsUpvote, Is.True);
        }

        [Test]
        public async Task VoteAsync_RemovesReaction_WhenSameVoteRepeated()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var reaction = TestDataSeeder.CreateReaction("user-2", post.Id, true);
            context.Reactions.Add(reaction);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            await service.VoteAsync(post.Id, "user-2", isUpvote: true);

            Assert.That(context.Reactions.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task VoteAsync_SwitchesVoteDirection_WhenDifferentVote()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var reaction = TestDataSeeder.CreateReaction("user-2", post.Id, true);
            context.Reactions.Add(reaction);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            await service.VoteAsync(post.Id, "user-2", isUpvote: false);

            Assert.That(context.Reactions.First().IsUpvote, Is.False);
        }

        [Test]
        public async Task VoteAsync_NotifiesPostOwner_OnNewUpvote()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var service = CreateService(context);
            await service.VoteAsync(post.Id, "user-2", isUpvote: true);

            _notifMock.Verify(n => n.CreateAsync(
                "user-1", "user-2", NotificationType.Reaction,
                post.Id, null, null), Times.Once);
        }

        [Test]
        public async Task VoteAsync_DoesNotNotify_OnDownvote()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var service = CreateService(context);
            await service.VoteAsync(post.Id, "user-2", isUpvote: false);

            _notifMock.Verify(n => n.CreateAsync(
                It.IsAny<string>(), It.IsAny<string>(), NotificationType.Reaction,
                It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Never);
        }

        [Test]
        public async Task VoteAsync_DoesNotNotify_WhenRemovingVote()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var reaction = TestDataSeeder.CreateReaction("user-2", post.Id, true);
            context.Reactions.Add(reaction);
            await context.SaveChangesAsync();

            var service = CreateService(context);
            await service.VoteAsync(post.Id, "user-2", isUpvote: true);

            _notifMock.Verify(n => n.CreateAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<NotificationType>(),
                It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Never);
        }

        [Test]
        public async Task VoteAsync_DoesNotNotify_WhenVoterIsPostOwner()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var service = CreateService(context);
            await service.VoteAsync(post.Id, "user-1", isUpvote: true);

            _notifMock.Verify(n => n.CreateAsync(
                It.IsAny<string>(), It.IsAny<string>(), NotificationType.Reaction,
                It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Never);
        }

        [Test]
        public async Task GetScoreAsync_ReturnsUpvotesMinusDownvotes()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var user3 = TestDataSeeder.CreateUser("user-3", "user3");
            var user4 = TestDataSeeder.CreateUser("user-4", "user4");
            context.Users.AddRange(user3, user4);
            await context.SaveChangesAsync();

            context.Reactions.AddRange(
                TestDataSeeder.CreateReaction("user-2", post.Id, true),
                TestDataSeeder.CreateReaction("user-3", post.Id, true),
                TestDataSeeder.CreateReaction("user-4", post.Id, false)
            );
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var score = await service.GetScoreAsync(post.Id);

            Assert.That(score, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserVoteAsync_ReturnsNull_WhenNoReactionExists()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var service = CreateService(context);
            var result = await service.GetUserVoteAsync(post.Id, "user-2");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetUserVoteAsync_ReturnsUp_WhenUpvoted()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            context.Reactions.Add(TestDataSeeder.CreateReaction("user-2", post.Id, true));
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var result = await service.GetUserVoteAsync(post.Id, "user-2");

            Assert.That(result, Is.EqualTo("up"));
        }

        [Test]
        public async Task GetUserVoteAsync_ReturnsDown_WhenDownvoted()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            context.Reactions.Add(TestDataSeeder.CreateReaction("user-2", post.Id, false));
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var result = await service.GetUserVoteAsync(post.Id, "user-2");

            Assert.That(result, Is.EqualTo("down"));
        }

        [Test]
        public async Task HasUserLikedAsync_ReturnsTrue_WhenReactionExists()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            context.Reactions.Add(TestDataSeeder.CreateReaction("user-2", post.Id, true));
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var result = await service.HasUserLikedAsync(post.Id, "user-2");

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetCountAsync_ReturnsCorrectCount()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var post = context.Posts.First();
            var user3 = TestDataSeeder.CreateUser("user-3", "user3");
            context.Users.Add(user3);
            await context.SaveChangesAsync();

            context.Reactions.AddRange(
                TestDataSeeder.CreateReaction("user-2", post.Id, true),
                TestDataSeeder.CreateReaction("user-3", post.Id, false)
            );
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var count = await service.GetCountAsync(post.Id);

            Assert.That(count, Is.EqualTo(2));
        }
    }
}