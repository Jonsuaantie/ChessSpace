using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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


    public IActionResult JoinGame(string gameCode) {
        var game = _context.Games.FirstOrDefault(g => g.GameCode == gameCode);
        if (game == null) {
            ViewBag.Error = "Game not found!";
            return View("JoinGame");
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
}