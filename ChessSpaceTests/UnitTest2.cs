using ChessSpace.Controllers;
using ChessSpace.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[TestFixture]
public class GameControllerTests {
    private AppDbContext _context;
    private GameController _controller;

    [SetUp]
    public void Setup() {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        _controller = new GameController(_context);

        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
    }

    [TearDown]
    public void TearDown() {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _controller.Dispose();
    }

    [Test]
    public void CreateGame_RedirectsToGameLobby_WithGeneratedGameCode() {
        // Act
        var result = _controller.CreateGame();

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult, Is.Not.Null);
        Assert.That(redirectResult.ActionName, Is.EqualTo("GameLobby"));

        var gameCode = redirectResult.RouteValues["gameCode"] as string;
        Assert.That(gameCode, Is.Not.Null);

        var game = _context.Games.FirstOrDefault(g => g.GameCode == gameCode);
        Assert.That(game, Is.Not.Null);
        Assert.That(game.GameCode, Is.EqualTo(gameCode));
    }

    [Test]
    public void JoinGame_Get_ReturnsView() {
        // Act
        var result = _controller.JoinGame();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
    }

    [Test]
    public void JoinGame_Post_RedirectsToGameLobby_WhenGameExists() {
        // Arrange
        var game = new Game { GameCode = "ABC123", CreatedAt = DateTime.UtcNow };
        _context.Games.Add(game);
        _context.SaveChanges();

        // Act
        var result = _controller.JoinGame("ABC123");

        // Assert
        var redirectResult = result as RedirectToActionResult;
        Assert.That(redirectResult, Is.Not.Null);
        Assert.That(redirectResult.ActionName, Is.EqualTo("GameLobby"));
        Assert.That(redirectResult.RouteValues["gameCode"], Is.EqualTo("ABC123"));
    }

    [Test]
    public void JoinGame_Post_ReturnsViewWithError_WhenGameDoesNotExist() {
        // Act
        var result = _controller.JoinGame("INVALID");

        // Assert
        var viewResult = result as ViewResult;
        Assert.That(viewResult, Is.Not.Null);
        Assert.That(_controller.ViewBag.Error, Is.EqualTo("Game not found!"));
    }


    [Test]
    public void GameLobby_ReturnsView_WhenGameExists() {
        // Arrange
        var game = new Game { GameCode = "ABC123", CreatedAt = DateTime.UtcNow };
        _context.Games.Add(game);
        _context.SaveChanges();

        // Act
        var result = _controller.GameLobby("ABC123");

        // Assert
        var viewResult = result as ViewResult;
        Assert.That(viewResult, Is.Not.Null);
        Assert.That(viewResult.Model, Is.EqualTo(game));
    }

    [Test]
    public void GameLobby_ReturnsNotFound_WhenGameDoesNotExist() {
        // Act
        var result = _controller.GameLobby("INVALID");

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void HomePage_ReturnsView_WithUserDetails_WhenUserIsAuthenticated() {
        // Arrange
        var player = new Player {
            UserId = 1,
            UserName = "TestUser",
            EloRating = 1500,
            Email = "test@example.com", 
            Password = "Password123!"   
        };
        _context.Players.Add(player);
        _context.SaveChanges();

        var mockHttpContext = new Mock<HttpContext>();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.NameIdentifier, "1")
    }));
        mockHttpContext.Setup(context => context.User).Returns(claimsPrincipal);

        _controller.ControllerContext = new ControllerContext {
            HttpContext = mockHttpContext.Object
        };

        // Act
        var result = _controller.HomePage();

        // Assert
        var viewResult = result as ViewResult;
        Assert.That(viewResult, Is.Not.Null);
        Assert.That(_controller.ViewData["UserName"], Is.EqualTo("TestUser"));
        Assert.That(_controller.ViewData["EloRating"], Is.EqualTo("1500"));
    }

    [Test]
    public void HomePage_ReturnsView_WithoutUserDetails_WhenUserIsNotAuthenticated() {
        // Arrange
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(context => context.User).Returns(new ClaimsPrincipal()); 

        _controller.ControllerContext = new ControllerContext {
            HttpContext = mockHttpContext.Object
        };

        // Act
        var result = _controller.HomePage();

        // Assert
        var viewResult = result as ViewResult;
        Assert.That(viewResult, Is.Not.Null);
        Assert.That(_controller.ViewData["UserName"], Is.Null);
        Assert.That(_controller.ViewData["EloRating"], Is.Null);
    }

}
