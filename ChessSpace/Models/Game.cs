using System.ComponentModel.DataAnnotations;

public class Game {
    [Key]
    public int GameId { get; set; }

    [Required]
    public string GameCode { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? BoardState { get; set; }  
    public string? CurrentPlayer { get; set; }  
}
