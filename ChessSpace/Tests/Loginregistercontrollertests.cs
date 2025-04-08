using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ChessSpace.Controllers;
using ChessSpace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace ChessSpace.Tests {
    [TestFixture]
    public class LoginRegisterControllerTests {
        private Mock<AppDbContext> _mockContext;
        private LoginRegisterController _controller;

        [SetUp]
        public void Setup() {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ChessSpaceTestDb")
                .Options;

            _mockContext = new Mock<AppDbContext>(options);
            _controller = new LoginRegisterController(_mockContext.Object);
        }

        [Test]
        public void Register_ReturnsView_WhenCalled() {
            var result = _controller.Register();
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Register_Post_ReturnsViewWithError_WhenEmailAlreadyExists() {
            // Arrange
            var existingPlayer = new Player { Email = "test@example.com", UserName = "TestPlayer" };
            _mockContext.Players.Add(existingPlayer);
            await _mockContext.SaveChangesAsync();

            var newPlayer = new Player { Email = "test@example.com", UserName = "NewPlayer", Password = "password123" };

            // Act
            var result = await _controller.Register(newPlayer);

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("An account with this email already exists.", viewResult.ViewData.ModelState["Email"].Errors[0].ErrorMessage);
        }

        [Test]
        public async Task VerifyCode_Post_ReturnsRedirectToHome_WhenCodeIsCorrect() {
            // Arrange
            var player = new Player { Email = "test@example.com", UserName = "TestPlayer", Password = "password123" };
            _mockContext.Players.Add(player);
            await _mockContext.SaveChangesAsync();

            TempData["VerificationCode"] = 123456;
            TempData["Player"] = Newtonsoft.Json.JsonConvert.SerializeObject(player);

            // Act
            var result = await _controller.VerifyCode(123456);

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

        [Test]
        public async Task Login_Post_ReturnsRedirectToHomePage_WhenLoginIsSuccessful() {
            // Arrange
            var player = new Player { Email = "test@example.com", Password = "password123", UserName = "TestPlayer" };
            _mockContext.Players.Add(player);
            await _mockContext.SaveChangesAsync();

            // Act
            var result = await _controller.Login("test@example.com", "password123");

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("HomePage", redirectResult.ActionName);
            Assert.AreEqual("Game", redirectResult.ControllerName);
        }

        [Test]
        public async Task Login_Post_ReturnsViewWithError_WhenLoginIsUnsuccessful() {
            // Act
            var result = await _controller.Login("test@example.com", "wrongpassword");

            // Assert
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Invalid login attempt.", viewResult.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        [Test]
        public async Task Logout_Post_ReturnsRedirectToHome_WhenCalled() {
            // Act
            var result = await _controller.Logout("TestPlayer");

            // Assert
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }
    }
}
