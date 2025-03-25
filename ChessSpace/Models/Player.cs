using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class Player {
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long, contain at least one number, one uppercase letter, and one special character.")]
    public string Password { get; set; }

    public int? EloRating { get; set; }
    public int? AvatarNumber { get; set; }
    public string? ChessStatus { get; set; }

    private static readonly PasswordHasher<Player> passwordHasher = new PasswordHasher<Player>();

    public void HashPassword() {
        Password = passwordHasher.HashPassword(this, Password);
    }

    public bool VerifyPassword(string password) {
        var result = passwordHasher.VerifyHashedPassword(this, Password, password);
        return result == PasswordVerificationResult.Success;
    }
}