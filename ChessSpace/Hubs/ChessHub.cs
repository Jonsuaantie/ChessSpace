using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class ChessHub : Hub {
    private readonly AppDbContext _context;

    public ChessHub(AppDbContext context) {
        _context = context;
    }
    public async Task SendMessage(string message, string gamecode) {
        await Clients.Group(gamecode).SendAsync("ReceiveMessage", message);
    }
    public async Task MovePiece(string updatedBoardHtml, string gamecode) {
        
        await Clients.Group(gamecode).SendAsync("UpdateBoard", updatedBoardHtml);
    }


    public async Task JoinGame(string gameCode) {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameCode);
        Console.WriteLine(Groups);
    }
}