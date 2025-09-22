using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

// 🔹 SQLite setup
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=ChessSpace.db"));

// 🔹 Authentication & Session
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config => {
        config.Cookie.Name = "UserLoginCookie";
        config.LoginPath = "/LoginRegister/Login";
        config.LogoutPath = "/LoginRegister/Logout";
    });

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

// 🔹 Automatisch migrations uitvoeren bij startup
using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapControllers();
app.MapHub<ChessHub>("/ChessHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
