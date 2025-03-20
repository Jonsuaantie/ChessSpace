using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }
}