using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

// 🔹 Connection string (Environment Variable of fallback)
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ChessSpace")
                       ?? "Host=mdmgobelhtzscxnhmqxo.supabase.co;Database=postgres;Username=postgres;Password=onbelangRyk!0;Port=5432;SSL Mode=Require;Trust Server Certificate=true";

// 🔹 DbContext registreren
builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseNpgsql(connectionString));

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
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
