using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace ChessSpace.Controllers {
    public class MailController : Controller {
        public IActionResult SendEmail() {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("chessspacenoreply@gmail.com"));
            email.To.Add(MailboxAddress.Parse("joshua.mobieltje@gmail.com"));
            email.Subject = "Je verificatiecode";

            var body = new TextPart("plain") {
                Text = $"Je 6-cijferige code is: {GenerateCode()}"
            };

            email.Body = body;

            try {
                using (var smtp = new SmtpClient()) {
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("chessspacenoreply@gmail.com", "denj yblu lbcw xejs");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Fout bij verzenden e-mail: {ex.Message}");
            }

            return RedirectToAction("Index", "Home");
        }

        private int GenerateCode() {
            return new Random().Next(100000, 999999);
        }
    }
}
