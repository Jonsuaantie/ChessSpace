using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class Player
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; }

    public int? EloRating { get; set; }
    public int? AvatarNumber { get; set; }
    public string? ChessStatus { get; set; }
}
