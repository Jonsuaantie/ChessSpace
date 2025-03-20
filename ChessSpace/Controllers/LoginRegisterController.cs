using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChessSpace.Controllers {
    public class LoginRegisterController : Controller {
        private readonly AppDbContext _context;

        public LoginRegisterController(AppDbContext context) {
            _context = context;
        }

        public IActionResult Login() {
            return View(); 
        }

        public IActionResult Register() {
            return View(); 
        }

        [HttpPost]
        public IActionResult Register(Player player, string source) {
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

}
