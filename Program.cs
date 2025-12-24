using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentACar_ip.Data;
using RentACar_ip.Hubs;
using RentACar_ip.Repositories;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// IDENTITY
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// SIGNALR
builder.Services.AddSignalR();

var app = builder.Build();


// ----------------------------
//    CREATE ADMIN + ROLE
// ----------------------------
app.MapGet("/create-admin", async (
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager) =>
{
    // ROL YOKSA OLUŞTUR
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    if (!await roleManager.RoleExistsAsync("Employee"))
        await roleManager.CreateAsync(new IdentityRole("Employee"));

    var email = "admin@test.com";
    var pass = "1234";

    var existing = await userManager.FindByEmailAsync(email);
    if (existing != null)
        return "Admin already exists.";

    var user = new IdentityUser
    {
        UserName = email,
        Email = email,
        EmailConfirmed = true
    };

    var result = await userManager.CreateAsync(user, pass);
    if (!result.Succeeded)
        return string.Join(", ", result.Errors.Select(e => e.Description));

    await userManager.AddToRoleAsync(user, "Admin");

    return "Admin created!";
});


// ---------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// SIGNALR HUB MAP
app.MapHub<ReservationHub>("/reservationHub");

app.MapRazorPages();   // Identity UI
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
