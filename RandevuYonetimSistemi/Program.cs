using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RandevuYonetimSistemi.Data;
using RandevuYonetimSistemi.Models;
using RandevuYonetimSistemi.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// ✅ PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Identity + Roles (DB tabloları, UserManager, SignInManager, RoleManager için)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Test için yumuşak şifre kuralları ("sau")
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ✅ [Authorize] login/denied yönlendirmeleri (Artık AuthController kullanıyoruz)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});

var app = builder.Build();

// ✅ Seed: Roles + Admin + Default Gym
await SeedAsync(app);

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

// ✅ Kısa yollar
app.MapControllerRoute(
    name: "admin_root",
    pattern: "Admin",
    defaults: new { controller = "Admin", action = "Index" });

app.MapControllerRoute(
    name: "member_root",
    pattern: "Member",
    defaults: new { controller = "Member", action = "Index" });

// ✅ Default: Portal
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Portal}/{id?}");

app.Run();

static async Task SeedAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // ✅ Roller
    string[] roles = { "Admin", "Member" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // ✅ Tek spor salonu (Default Gym)
    if (!db.Gyms.Any())
    {
        db.Gyms.Add(new Gym
        {
            Name = "Sakarya Fitness Center",
            OpenTime = new TimeOnly(9, 0),
            CloseTime = new TimeOnly(22, 0),
            Address = "Sakarya",
            Phone = "0000000000"
        });

        await db.SaveChangesAsync();
    }

    // ✅ Admin kullanıcı (seed)
    var adminEmail = "b231210048@sakarya.edu.tr";
    var adminPassword = "sau";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser is null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FullName = "Admin"
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new Exception("Admin user create failed: " + errors);
        }
    }

    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        await userManager.AddToRoleAsync(adminUser, "Admin");
}
