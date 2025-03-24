using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

public class GameController : Controller {
    private readonly AppDbContext _context;

    public GameController(AppDbContext context) {
        _context = context;
    }

    public IActionResult CreateGame() {
        string gameCode = GenerateGameCode();
        var game = new Game { GameCode = gameCode, CreatedAt = DateTime.UtcNow };
        _context.Games.Add(game);
        _context.SaveChanges();

        return RedirectToAction("GameLobby", new { gameCode = game.GameCode });
    }

    [HttpGet]
    public IActionResult JoinGame() {
        return View();
    }

    [HttpPost]
    public IActionResult JoinGame(string gameCode) {
        var game = _context.Games.FirstOrDefault(g => g.GameCode == gameCode);

        if (game == null) {
            ViewBag.Error = "Game not found!";
            return View();
        }

        return RedirectToAction("GameLobby", new { gameCode = game.GameCode });
    }

    public IActionResult GameLobby(string gameCode) {
        var game = _context.Games.FirstOrDefault(g => g.GameCode == gameCode);
        if (game == null) {
            return NotFound();
        }
        return View(game);
    }

    private string GenerateGameCode() {
        return Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
    }

    public IActionResult HomePage() {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId != null) {
            var player = _context.Players.SingleOrDefault(p => p.UserId.ToString() == userId);

            if (player != null) {
                ViewData["UserName"] = player.UserName;
                ViewData["EloRating"] = player.EloRating.HasValue ? player.EloRating.Value.ToString() : "N/A";
            }
        }

        return View();
    }
}