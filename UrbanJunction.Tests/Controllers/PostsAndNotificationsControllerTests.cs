using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using UrbanJunction.Data.Models;
using UrbanJunction.Data.ViewModels;
using UrbanJunction.Services.Interfaces;
using UrbanJunction.Tests.Helpers;

namespace UrbanJunction.Tests.Controllers
{
    [TestFixture]
    public class PostsControllerTests
    {
        private Mock<IPostService> _postServiceMock = null!;
        private Mock<IReactionService> _reactionServiceMock = null!;
        private Mock<ICommentService> _commentServiceMock = null!;
        private Mock<ITagService> _tagServiceMock = null!;

        [SetUp]
        public void SetUp()
        {
            _postServiceMock     = new Mock<IPostService>();
            _reactionServiceMock = new Mock<IReactionService>();
            _commentServiceMock  = new Mock<ICommentService>();
            _tagServiceMock      = new Mock<ITagService>();
        }

        private PostsController CreateController(string userId = "user-1", bool isAdmin = false)
        {
            var context = TestDbContextFactory.Create();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, "testuser")
            };
            if (isAdmin) claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            var identity   = new ClaimsIdentity(claims, "TestAuth");
            var principal  = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };

            var controller = new PostsController(
                _postServiceMock.Object,
                _reactionServiceMock.Object,
                _commentServiceMock.Object,
                context,
                _tagServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };

            return controller;
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenPostDoesNotExist()
        {
            _postServiceMock.Setup(s => s.GetDetailsAsync(999))
                .ReturnsAsync((Post?)null);

            var controller = CreateController();
            var result = await controller.Details(999);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Details_ReturnsView_WhenPostExists()
        {
            var post = TestDataSeeder.CreatePost();
            post.Subcategory = TestDataSeeder.CreateSubcategory();
            post.Subcategory.Topic = TestDataSeeder.CreateTopic();

            _postServiceMock.Setup(s => s.GetDetailsAsync(1)).ReturnsAsync(post);
            _reactionServiceMock.Setup(s => s.GetScoreAsync(1)).ReturnsAsync(5);
            _reactionServiceMock.Setup(s => s.GetUserVoteAsync(1, It.IsAny<string>())).ReturnsAsync("up");

            var controller = CreateController();
            var result = await controller.Details(1);

            Assert.That(result, Is.InstanceOf<ViewResult>());
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            _postServiceMock.Setup(s => s.DeleteAsync(999, "user-1", false))
                .ReturnsAsync(false);

            var controller = CreateController();
            var result = await controller.Delete(999);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_Redirects_WhenSuccessful()
        {
            _postServiceMock.Setup(s => s.DeleteAsync(1, "user-1", false))
                .ReturnsAsync(true);

            var controller = CreateController();
            var result = await controller.Delete(1);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirect = (RedirectToActionResult)result;
            Assert.That(redirect.ActionName, Is.EqualTo("MyPosts"));
        }

        [Test]
        public async Task Vote_ReturnsJson_WithScoreAndUserVote()
        {
            _reactionServiceMock.Setup(s => s.VoteAsync(1, "user-1", true))
                .Returns(Task.CompletedTask);
            _reactionServiceMock.Setup(s => s.GetScoreAsync(1)).ReturnsAsync(3);
            _reactionServiceMock.Setup(s => s.GetUserVoteAsync(1, "user-1")).ReturnsAsync("up");

            var controller = CreateController();
            var result = await controller.Vote(1, true);

            Assert.That(result, Is.InstanceOf<JsonResult>());
            var json = (JsonResult)result;
            var score    = json.Value!.GetType().GetProperty("score")!.GetValue(json.Value);
            var userVote = json.Value!.GetType().GetProperty("userVote")!.GetValue(json.Value);
            Assert.That(score, Is.EqualTo(3));
            Assert.That(userVote, Is.EqualTo("up"));
        }

        [Test]
        public async Task AddComment_Redirects_ToDetails()
        {
            _commentServiceMock
                .Setup(s => s.AddAsync(1, "Test comment", "user-1", null))
                .Returns(Task.CompletedTask);

            var controller = CreateController();
            var result = await controller.AddComment(1, "Test comment", null);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirect = (RedirectToActionResult)result;
            Assert.That(redirect.ActionName, Is.EqualTo("Details"));
        }

        [Test]
        public async Task MyPosts_ReturnsView_WithUserPosts()
        {
            var posts = new List<Post> { TestDataSeeder.CreatePost() };
            _postServiceMock.Setup(s => s.GetByUserAsync("user-1")).ReturnsAsync(posts);
            _reactionServiceMock.Setup(s => s.GetUserVoteAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((string?)null);

            var controller = CreateController();
            var result = await controller.MyPosts();

            Assert.That(result, Is.InstanceOf<ViewResult>());
            var view = (ViewResult)result;
            Assert.That(view.Model, Is.InstanceOf<IEnumerable<Post>>());
        }
    }

    [TestFixture]
    public class NotificationsControllerTests
    {
        private Mock<INotificationService> _notifServiceMock = null!;

        [SetUp]
        public void SetUp()
        {
            _notifServiceMock = new Mock<INotificationService>();
        }

        private NotificationsController CreateController(string userId = "user-1")
        {
            var claims     = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity   = new ClaimsIdentity(claims, "TestAuth");
            var principal  = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };

            return new NotificationsController(_notifServiceMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };
        }

        [Test]
        public async Task UnreadCount_ReturnsJson_WithCorrectCount()
        {
            _notifServiceMock.Setup(s => s.GetUnreadCountAsync("user-1")).ReturnsAsync(5);

            var controller = CreateController();
            var result = await controller.UnreadCount();

            Assert.That(result, Is.InstanceOf<JsonResult>());
            var json = (JsonResult)result;
            var count = json.Value!.GetType().GetProperty("count")!.GetValue(json.Value);
            Assert.That(count, Is.EqualTo(5));
        }

        [Test]
        public async Task MarkRead_ReturnsJson_WithUpdatedCount()
        {
            _notifServiceMock.Setup(s => s.MarkReadAsync(1, "user-1")).Returns(Task.CompletedTask);
            _notifServiceMock.Setup(s => s.GetUnreadCountAsync("user-1")).ReturnsAsync(3);

            var controller = CreateController();
            var result = await controller.MarkRead(1);

            Assert.That(result, Is.InstanceOf<JsonResult>());
            var json = (JsonResult)result;
            var unreadCount = json.Value!.GetType().GetProperty("unreadCount")!.GetValue(json.Value);
            Assert.That(unreadCount, Is.EqualTo(3));
        }

        [Test]
        public async Task MarkAllRead_ReturnsJson_WithZeroCount()
        {
            _notifServiceMock.Setup(s => s.MarkAllReadAsync("user-1")).Returns(Task.CompletedTask);

            var controller = CreateController();
            var result = await controller.MarkAllRead();

            Assert.That(result, Is.InstanceOf<JsonResult>());
            var json = (JsonResult)result;
            var unreadCount = json.Value!.GetType().GetProperty("unreadCount")!.GetValue(json.Value);
            Assert.That(unreadCount, Is.EqualTo(0));
        }

        [Test]
        public async Task Dropdown_ReturnsPartialView_WithNotifications()
        {
            var notifications = new List<Notification>();
            _notifServiceMock.Setup(s => s.GetForUserAsync("user-1", 20)).ReturnsAsync(notifications);
            _notifServiceMock.Setup(s => s.GetUnreadCountAsync("user-1")).ReturnsAsync(0);

            var controller = CreateController();
            var result = await controller.Dropdown();

            Assert.That(result, Is.InstanceOf<PartialViewResult>());
            var partial = (PartialViewResult)result;
            Assert.That(partial.ViewName, Is.EqualTo("_NotificationDropdown"));
        }
    }
}
