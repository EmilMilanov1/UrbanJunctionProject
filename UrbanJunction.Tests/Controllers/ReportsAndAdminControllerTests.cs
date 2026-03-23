using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using UrbanJunction.Data.Models;
using UrbanJunction.Services.Interfaces;
using UrbanJunction.Tests.Helpers;

namespace UrbanJunction.Tests.Controllers
{
    [TestFixture]
    public class ReportsControllerTests
    {
        private ReportsController CreateController(
            UrbanJunction.Data.ApplicationDbContext context,
            string userId = "user-2")
        {
            var claims     = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity   = new ClaimsIdentity(claims, "TestAuth");
            var principal  = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };

            return new ReportsController(context)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };
        }

        [Test]
        public async Task Submit_SavesReport_AndRedirects()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var controller = CreateController(context);
            var result = await controller.Submit(ReportReason.Spam, postId: 1, null, null, null);

            Assert.That(context.Reports.Count(), Is.EqualTo(1));
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        }

        [Test]
        public async Task Submit_DoesNotSaveDuplicate_WhenPendingExists()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            var controller = CreateController(context);
            await controller.Submit(ReportReason.Spam, postId: 1, null, null, null);
            await controller.Submit(ReportReason.Spam, postId: 1, null, null, null);

            Assert.That(context.Reports.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task Resolve_ReturnsNotFound_WhenReportMissing()
        {
            var context = TestDbContextFactory.Create();
            var controller = CreateController(context);

            var result = await controller.Resolve(999);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Resolve_UpdatesStatusToResolved()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            context.Reports.Add(new Report
            {
                Id = 1,
                ReporterId = "user-2",
                Reason = ReportReason.Spam,
                Status = ReportStatus.Pending,
                CreatedOn = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var controller = CreateController(context, "user-2");
            await controller.Resolve(1);

            Assert.That(context.Reports.First().Status, Is.EqualTo(ReportStatus.Resolved));
        }

        [Test]
        public async Task Dismiss_UpdatesStatusToDismissed()
        {
            var context = TestDbContextFactory.Create();
            await TestDataSeeder.SeedBasicDataAsync(context);

            context.Reports.Add(new Report
            {
                Id = 1,
                ReporterId = "user-2",
                Reason = ReportReason.Spam,
                Status = ReportStatus.Pending,
                CreatedOn = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var controller = CreateController(context, "user-2");
            await controller.Dismiss(1);

            Assert.That(context.Reports.First().Status, Is.EqualTo(ReportStatus.Dismissed));
        }

        [Test]
        public async Task Dismiss_ReturnsNotFound_WhenReportMissing()
        {
            var context = TestDbContextFactory.Create();
            var controller = CreateController(context);

            var result = await controller.Dismiss(999);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }

    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<IAdminService> _adminServiceMock = null!;
        private Mock<UserManager<UrbanUser>> _userManagerMock = null!;

        [SetUp]
        public void SetUp()
        {
            _adminServiceMock = new Mock<IAdminService>();
            var store = new Mock<IUserStore<UrbanUser>>();
            _userManagerMock = new Mock<UserManager<UrbanUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }

        private AdminController CreateController(UrbanJunction.Data.ApplicationDbContext context)
        {
            var claims     = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "admin-1"), new Claim(ClaimTypes.Role, "Admin") };
            var identity   = new ClaimsIdentity(claims, "TestAuth");
            var principal  = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = principal };

            return new AdminController(_adminServiceMock.Object, context, _userManagerMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };
        }

        [Test]
        public async Task DeletePost_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            var context = TestDbContextFactory.Create();
            _adminServiceMock.Setup(s => s.DeletePostAsync(999)).ReturnsAsync(false);

            var controller = CreateController(context);
            var result = await controller.DeletePost(999);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task DeletePost_Redirects_WhenSuccessful()
        {
            var context = TestDbContextFactory.Create();
            _adminServiceMock.Setup(s => s.DeletePostAsync(1)).ReturnsAsync(true);

            var controller = CreateController(context);
            var result = await controller.DeletePost(1);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        }

        [Test]
        public async Task BanUser_ReturnsNotFound_WhenServiceReturnsFalse()
        {
            var context = TestDbContextFactory.Create();
            _adminServiceMock.Setup(s => s.BanUserAsync("nonexistent")).ReturnsAsync(false);

            var controller = CreateController(context);
            var result = await controller.BanUser("nonexistent");

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task BanUser_Redirects_WhenSuccessful()
        {
            var context = TestDbContextFactory.Create();
            _adminServiceMock.Setup(s => s.BanUserAsync("user-1")).ReturnsAsync(true);

            var controller = CreateController(context);
            var result = await controller.BanUser("user-1");

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        }

        [Test]
        public async Task TogglePin_ReturnsNotFound_WhenPostMissing()
        {
            var context = TestDbContextFactory.Create();
            var controller = CreateController(context);

            var result = await controller.TogglePin(999, null);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task ToggleLock_ReturnsNotFound_WhenPostMissing()
        {
            var context = TestDbContextFactory.Create();
            var controller = CreateController(context);

            var result = await controller.ToggleLock(999, null);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
