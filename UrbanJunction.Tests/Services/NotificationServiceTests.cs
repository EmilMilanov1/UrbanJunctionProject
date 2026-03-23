using NUnit.Framework;
using UrbanJunction.Data.Models;
using UrbanJunction.Tests.Helpers;

namespace UrbanJunction.Tests.Services
{
    [TestFixture]
    public class NotificationServiceTests
    {
        private NotificationService CreateService(UrbanJunction.Data.ApplicationDbContext context)
        {
            return new NotificationService(context);
        }

        [Test]
        public async Task CreateAsync_SavesNotification_WhenValid()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            await service.CreateAsync("user-1", "user-2", NotificationType.Follow);

            Assert.That(context.Notifications.Count(), Is.EqualTo(1));
            var notif = context.Notifications.First();
            Assert.That(notif.UserId, Is.EqualTo("user-1"));
            Assert.That(notif.ActorId, Is.EqualTo("user-2"));
            Assert.That(notif.Type, Is.EqualTo(NotificationType.Follow));
            Assert.That(notif.IsRead, Is.False);
        }

        [Test]
        public async Task CreateAsync_DoesNotSave_WhenActorEqualsRecipient()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            await service.CreateAsync("user-1", "user-1", NotificationType.Comment);

            Assert.That(context.Notifications.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task CreateAsync_DoesNotSave_WhenDuplicateUnreadExists()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            await service.CreateAsync("user-1", "user-2", NotificationType.Follow);
            await service.CreateAsync("user-1", "user-2", NotificationType.Follow);

            Assert.That(context.Notifications.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task CreateAsync_AllowsDuplicate_WhenFirstIsRead()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            await service.CreateAsync("user-1", "user-2", NotificationType.Follow);

            // Mark as read
            context.Notifications.First().IsRead = true;
            await context.SaveChangesAsync();

            // Now create again — should be allowed
            await service.CreateAsync("user-1", "user-2", NotificationType.Follow);

            Assert.That(context.Notifications.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetUnreadCountAsync_ReturnsCorrectCount()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            context.Notifications.AddRange(
                new Notification { UserId = "user-1", ActorId = "user-2", Type = NotificationType.Follow, IsRead = false, CreatedOn = DateTime.UtcNow },
                new Notification { UserId = "user-1", ActorId = "user-2", Type = NotificationType.Comment, IsRead = false, CreatedOn = DateTime.UtcNow },
                new Notification { UserId = "user-1", ActorId = "user-2", Type = NotificationType.Reaction, IsRead = true, CreatedOn = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var count = await service.GetUnreadCountAsync("user-1");

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetUnreadCountAsync_ReturnsZero_WhenNoNotifications()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var service = CreateService(context);
            var count = await service.GetUnreadCountAsync("user-1");

            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public async Task MarkReadAsync_MarksNotificationAsRead()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            context.Notifications.Add(new Notification
            {
                Id = 1,
                UserId = "user-1",
                ActorId = "user-2",
                Type = NotificationType.Follow,
                IsRead = false,
                CreatedOn = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);
            await service.MarkReadAsync(1, "user-1");

            Assert.That(context.Notifications.First().IsRead, Is.True);
        }

        [Test]
        public async Task MarkReadAsync_DoesNothing_WhenWrongUser()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            context.Notifications.Add(new Notification
            {
                Id = 1,
                UserId = "user-1",
                ActorId = "user-2",
                Type = NotificationType.Follow,
                IsRead = false,
                CreatedOn = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var service = CreateService(context);
            await service.MarkReadAsync(1, "user-2"); // wrong user

            Assert.That(context.Notifications.First().IsRead, Is.False);
        }

        [Test]
        public async Task MarkAllReadAsync_MarksAllUnreadNotificationsAsRead()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            context.Notifications.AddRange(
                new Notification { UserId = "user-1", ActorId = "user-2", Type = NotificationType.Follow,   IsRead = false, CreatedOn = DateTime.UtcNow },
                new Notification { UserId = "user-1", ActorId = "user-2", Type = NotificationType.Comment,  IsRead = false, CreatedOn = DateTime.UtcNow },
                new Notification { UserId = "user-1", ActorId = "user-2", Type = NotificationType.Reaction, IsRead = true,  CreatedOn = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();

            var service = CreateService(context);
            await service.MarkAllReadAsync("user-1");

            Assert.That(context.Notifications.All(n => n.IsRead), Is.True);
        }

        [Test]
        public async Task GetForUserAsync_ReturnsOnlyUserNotifications()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            context.Notifications.AddRange(
                new Notification { UserId = "user-1", ActorId = "user-2", Type = NotificationType.Follow,  IsRead = false, CreatedOn = DateTime.UtcNow },
                new Notification { UserId = "user-2", ActorId = "user-1", Type = NotificationType.Comment, IsRead = false, CreatedOn = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var results = await service.GetForUserAsync("user-1");

            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.First().UserId, Is.EqualTo("user-1"));
        }

        [Test]
        public async Task GetForUserAsync_RespectsMaxTakeLimit()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            for (int i = 0; i < 25; i++)
            {
                context.Notifications.Add(new Notification
                {
                    UserId = "user-1",
                    ActorId = "user-2",
                    Type = NotificationType.Comment,
                    IsRead = false,
                    CreatedOn = DateTime.UtcNow.AddMinutes(-i)
                });
            }
            await context.SaveChangesAsync();

            var service = CreateService(context);
            var results = await service.GetForUserAsync("user-1", take: 10);

            Assert.That(results.Count(), Is.EqualTo(10));
        }
    }
}
