using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace ChessSpace.Controllers {
    public class LoginRegisterController : Controller {
        private readonly AppDbContext _context;

        public LoginRegisterController(AppDbContext context) {
            _context = context;
        }

        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Player player) {
            if (ModelState.IsValid) {
                if (_context.Players.Any(p => p.Email == player.Email)) {
                    ModelState.AddModelError("Email", "An account with this email already exists.");
                    return View(player);
                }

                player.HashPassword();

                int verificationCode = new Random().Next(100000, 999999);
                TempData["VerificationCode"] = verificationCode;
                TempData["Player"] = JsonConvert.SerializeObject(player);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("chessspacenoreply@gmail.com"));
                email.To.Add(MailboxAddress.Parse(player.Email));
                email.Subject = "Your verification code";

                var body = new TextPart("plain") {
                    Text = $"Your 6-digit verification code is: {verificationCode}"
                };

                email.Body = body;

                try {
                    using (var smtp = new SmtpClient()) {
                        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                        smtp.Authenticate("chessspacenoreply@gmail.com", "sbln feoi atsy rddn");
                        smtp.Send(email);
                        smtp.Disconnect(true);
                    }
                    TempData["Notification"] = "⚠️A verification email has been sent to your email address.";
                }
                catch (Exception ex) {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    TempData["Notification"] = "⚠️Failed to send verification email. Please try again.";
                }

                return RedirectToAction("VerifyCode");
            }
            return View(player);
        }

        public IActionResult VerifyCode() {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyCode(int code) {
            if (TempData["VerificationCode"] != null) {
                int verificationCode = (int)TempData["VerificationCode"];
                if (code == verificationCode) {
                    var playerJson = TempData["Player"] as string;
                    var player = JsonConvert.DeserializeObject<Player>(playerJson);
                    if (player != null) {
                        _context.Players.Add(player);
                        _context.SaveChanges();
                        TempData["Notification"] = "⚠️Your account has been successfully registered.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["Notification"] = "⚠️Invalid verification code!";
            }
            else {
                TempData["Notification"] = "⚠️Verification code is missing!";
            }

            TempData.Keep("VerificationCode");
            TempData.Keep("Player");

            return View();
        }

        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password) {
            var player = _context.Players.SingleOrDefault(p => p.Email == email);
            if (player != null && player.VerifyPassword(password)) {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, player.UserId.ToString()),
                    new Claim(ClaimTypes.Name, player.UserName),
                    new Claim(ClaimTypes.Email, player.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("HomePage", "Game");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string name) {
            string Name = name;   
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}