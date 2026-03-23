using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using UrbanJunction.Data.ViewModels;
using UrbanJunction.Web.Models;

namespace UrbanJunction.Tests.ViewModels
{
    [TestFixture]
    public class PostFormViewModelTests
    {
        private static List<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(model);
            Validator.TryValidateObject(model, ctx, results, true);
            return results;
        }

        [Test]
        public void PostFormViewModel_IsValid_WithCorrectData()
        {
            var model = new PostFormViewModel
            {
                Title = "Valid Title",
                Content = "Valid content here",
                SubcategoryId = 1
            };

            var errors = Validate(model);
            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void PostFormViewModel_IsInvalid_WhenTitleIsEmpty()
        {
            var model = new PostFormViewModel
            {
                Title = "",
                Content = "Some content",
                SubcategoryId = 1
            };

            var errors = Validate(model);
            Assert.That(errors.Any(e => e.MemberNames.Contains("Title")), Is.True);
        }

        [Test]
        public void PostFormViewModel_IsInvalid_WhenTitleExceedsMaxLength()
        {
            var model = new PostFormViewModel
            {
                Title = new string('a', 101), // max is 100
                Content = "Some content",
                SubcategoryId = 1
            };

            var errors = Validate(model);
            Assert.That(errors.Any(e => e.MemberNames.Contains("Title")), Is.True);
        }

        [Test]
        public void PostFormViewModel_IsInvalid_WhenContentIsEmpty()
        {
            var model = new PostFormViewModel
            {
                Title = "Valid Title",
                Content = "",
                SubcategoryId = 1
            };

            var errors = Validate(model);
            Assert.That(errors.Any(e => e.MemberNames.Contains("Content")), Is.True);
        }

        [Test]
        public void PostFormViewModel_IsInvalid_WhenSubcategoryIdIsZero()
        {
            var model = new PostFormViewModel
            {
                Title = "Valid Title",
                Content = "Valid content",
                SubcategoryId = 0
            };

            var errors = Validate(model);
            Assert.That(errors.Any(e => e.MemberNames.Contains("SubcategoryId")), Is.True);
        }
    }

    [TestFixture]
    public class RegisterViewModelTests
    {
        private static List<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(model);
            Validator.TryValidateObject(model, ctx, results, true);
            return results;
        }

        [Test]
        public void RegisterViewModel_IsValid_WithCorrectData()
        {
            var model = new RegisterViewModel
            {
                Username = "validuser",
                Email = "valid@email.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            var errors = Validate(model);
            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void RegisterViewModel_IsInvalid_WhenPasswordTooShort()
        {
            var model = new RegisterViewModel
            {
                Username = "validuser",
                Email = "valid@email.com",
                Password = "abc",
                ConfirmPassword = "abc"
            };

            var errors = Validate(model);
            Assert.That(errors.Any(e => e.MemberNames.Contains("Password")), Is.True);
        }

        [Test]
        public void RegisterViewModel_IsInvalid_WhenEmailIsInvalid()
        {
            var model = new RegisterViewModel
            {
                Username = "validuser",
                Email = "notanemail",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            var errors = Validate(model);
            Assert.That(errors.Any(e => e.MemberNames.Contains("Email")), Is.True);
        }

        [Test]
        public void RegisterViewModel_IsInvalid_WhenUsernameTooShort()
        {
            var model = new RegisterViewModel
            {
                Username = "ab", // min 3
                Email = "valid@email.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            var errors = Validate(model);
            Assert.That(errors.Any(e => e.MemberNames.Contains("Username")), Is.True);
        }

        [Test]
        public void RegisterViewModel_IsInvalid_WhenUsernameTooLong()
        {
            var model = new RegisterViewModel
            {
                Username = new string('a', 21), // max 20
                Email = "valid@email.com",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };

            var errors = Validate(model);
            Assert.That(errors.Any(e => e.MemberNames.Contains("Username")), Is.True);
        }
    }

    [TestFixture]
    public class LoginViewModelTests
    {
        private static List<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(model);
            Validator.TryValidateObject(model, ctx, results, true);
            return results;
        }

        [Test]
        public void LoginViewModel_IsValid_WithCorrectData()
        {
            var model = new LoginViewModel
            {
                UsernameOrEmail = "testuser",
                Password = "password123"
            };

            var errors = Validate(model);
            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void LoginViewModel_IsInvalid_WhenUsernameOrEmailIsEmpty()
        {
            var model = new LoginViewModel
            {
                UsernameOrEmail = "",
                Password = "password123"
            };

            var errors = Validate(model);
            Assert.That(errors.Any(e => e.MemberNames.Contains("UsernameOrEmail")), Is.True);
        }

        [Test]
        public void LoginViewModel_IsInvalid_WhenPasswordIsEmpty()
        {
            var model = new LoginViewModel
            {
                UsernameOrEmail = "testuser",
                Password = ""
            };

            var errors = Validate(model);
            Assert.That(errors.Any(e => e.MemberNames.Contains("Password")), Is.True);
        }
    }
}
