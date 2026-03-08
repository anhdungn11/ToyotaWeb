using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using System.Globalization;
var culture = new CultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// ================= DATABASE =================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ================= IDENTITY =================
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>() // Thêm Role
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ================= SEED ROLE + ADMIN =================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = { "Admin", "User" };

    // 1️⃣ TẠO ROLE TRƯỚC
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }


    // 2️⃣ TẠO ADMIN USER
    string adminEmail = "admin@gmail.com";
    string adminPassword = "123456";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

if (adminUser == null)
{
    var newAdmin = new IdentityUser
    {
        UserName = adminEmail,
        Email = adminEmail,
        EmailConfirmed = true
    };

    var result = await userManager.CreateAsync(newAdmin, adminPassword);

    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(newAdmin, "Admin");
    }
}
else
{
    // Nếu user đã tồn tại nhưng chưa có role
    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}
}

// ================= PIPELINE =================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();   // BẮT BUỘC có
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "carDetails",
    pattern: "cars/{slug}",
    defaults: new { controller = "Cars", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Identity cần cái này


app.Run();