using ChessSpace.Controllers;
using ChessSpace.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[TestFixture]
public class LoginRegisterControllerTests {
    private AppDbContext _context;
    private LoginRegisterController _controller;

    [SetUp]
    public void Setup() {
        // Set up in-memory database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        // Initialize Controller
        _controller = new LoginRegisterController(_context);

        // Mock TempData
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
    }

    [TearDown]
    public void TearDown() {
        // Dispose of the in-memory database and controller
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _controller.Dispose();
    }

    [Test]
    public void Register_ReturnsView_WhenCalled() {
        // Act
        var result = _controller.Register();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
    }

    [Test]
    public void VerifyCode_Post_ReturnsRedirectToHome_WhenCodeIsCorrect() {
        // Arrange
        var player = new Player { Email = "test@example.com", UserName = "TestPlayer", Password = "password123" };
        _controller.TempData["VerificationCode"] = 123456;
        _controller.TempData["Player"] = JsonConvert.SerializeObject(player);

        // Act
        var result = _controller.VerifyCode(123456);

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult, Is.Not.Null);
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
    }

    [Test]
    public async Task Login_Post_ReturnsRedirectToHomePage_WhenLoginIsSuccessful() {
        // Arrange
        var player = new Player { Email = "test@example.com", Password = "password123", UserName = "TestPlayer" };
        player.HashPassword();
        _context.Players.Add(player);
        _context.SaveChanges();

        // Mock HttpContext and SignInAsync
        var mockHttpContext = new Mock<HttpContext>();
        var mockAuthenticationService = new Mock<IAuthenticationService>();
        mockAuthenticationService
            .Setup(auth => auth.SignInAsync(
                It.IsAny<HttpContext>(),
                It.IsAny<string>(),
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        mockHttpContext
            .Setup(context => context.RequestServices.GetService(typeof(IAuthenticationService)))
            .Returns(mockAuthenticationService.Object);

        // Mock IUrlHelper
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(url => url.Action(It.IsAny<UrlActionContext>()))
            .Returns("HomePage");

        _controller.ControllerContext = new ControllerContext {
            HttpContext = mockHttpContext.Object
        };

        _controller.Url = mockUrlHelper.Object;

        // Act
        var result = await _controller.Login("test@example.com", "password123");

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult, Is.Not.Null);
        Assert.That(redirectResult.ActionName, Is.EqualTo("HomePage"));
        Assert.That(redirectResult.ControllerName, Is.EqualTo("Game"));
    }


    [Test]
    public async Task Login_Post_ReturnsViewWithError_WhenLoginIsUnsuccessful() {
        // Act
        var result = await _controller.Login("test@example.com", "wrongpassword");

        // Assert
        var viewResult = result as ViewResult;
        Assert.That(viewResult, Is.Not.Null);
        Assert.That(_controller.ModelState[string.Empty].Errors[0].ErrorMessage, Is.EqualTo("Invalid login attempt."));
    }

    [Test]
    public async Task Logout_Post_ReturnsRedirectToHome_WhenCalled() {
        // Mock HttpContext and SignOutAsync
        var mockHttpContext = new Mock<HttpContext>();
        var mockAuthenticationService = new Mock<IAuthenticationService>();
        mockAuthenticationService
            .Setup(auth => auth.SignOutAsync(
                It.IsAny<HttpContext>(),
                It.IsAny<string?>(),
                It.IsAny<AuthenticationProperties?>()))
            .Returns(Task.CompletedTask);

        mockHttpContext
            .Setup(context => context.RequestServices.GetService(typeof(IAuthenticationService)))
            .Returns(mockAuthenticationService.Object);

        // Mock IUrlHelper
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(url => url.Action(It.IsAny<UrlActionContext>()))
            .Returns("Index");

        _controller.ControllerContext = new ControllerContext {
            HttpContext = mockHttpContext.Object
        };

        _controller.Url = mockUrlHelper.Object;

        // Act
        var result = await _controller.Logout("TestPlayer");

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult, Is.Not.Null);
        Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
        Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
    }

}
