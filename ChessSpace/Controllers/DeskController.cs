using Microsoft.AspNetCore.Mvc;

namespace ChessSpace.Controllers {
    public class DeskController : Controller {
        public IActionResult JOS() {
            return View();
        }
        public IActionResult Desktop() {
            return View();
        }
        public IActionResult Chessspace() {
            return View();
        }
    }
}
