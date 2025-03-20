using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class PlayersController : Controller
{
    private readonly AppDbContext _context;

    public PlayersController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var players = _context.Players.ToList();
        return View(players);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Player player, string source) {
        if (ModelState.IsValid) {
            _context.Players.Add(player);
            _context.SaveChanges();

            if (source == "Register") {
                return RedirectToAction("Welcome", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
        return View(player);
    }


}
