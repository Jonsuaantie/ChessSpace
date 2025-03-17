using System.ComponentModel.DataAnnotations;

public class Game {
    [Key]
    public int GameId { get; set; }

    [Required]
    public string GameCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? BoardState { get; set; }  // Een string die de huidige staat van het bord bijhoudt
    public string? CurrentPlayer { get; set; }  // Wie is aan de beurt? ("Player1" of "Player2")
}
