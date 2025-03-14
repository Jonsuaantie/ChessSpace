using Microsoft.AspNetCore.Mvc;

namespace ChessSpace.Controllers {
    public class LoginRegisterController : Controller {
        public IActionResult Login() {
            return View(); 
        }

        public IActionResult Register() {
            return View(); 
        }
    }

}
