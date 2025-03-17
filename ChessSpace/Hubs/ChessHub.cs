using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ChessHub : Hub {
    private readonly AppDbContext _context;

    public ChessHub(AppDbContext context) {
        _context = context;
    }
    public async Task SendMessage(string message) {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
    public async Task MovePiece(string gameCode, string fromCell, string toCell) {
        // Haal het spel op uit de database
        var game = _context.Games.FirstOrDefault(g => g.GameCode == gameCode);

        if (game == null) {
            return;
        }

        // Update de beurt
        game.CurrentPlayer = game.CurrentPlayer == "Player1" ? "Player2" : "Player1";
        _context.SaveChanges();

        // Verstuur de zet naar de andere speler
        await Clients.Group(gameCode).SendAsync("PieceMoved", fromCell, toCell);
    }

    // Deze methode voegt een speler toe aan de gamegroep
    public async Task JoinGame(string gameCode) {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameCode);
    }
}