using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ToyotaWeb.Data;
using ToyotaWeb.Models;
using ToyotaWeb.Services;
using System.Globalization;
using DinkToPdf;
using DinkToPdf.Contracts; 
var culture = new CultureInfo("vi-VN");

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var builder = WebApplication.CreateBuilder(args);

// ================= PORT =================

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

builder.WebHost.UseUrls($"http://*:{port}");


// ================= DATABASE =================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


// ================= SERVICES =================

builder.Services.AddHttpClient<GeminiService>();

builder.Services.AddSingleton(typeof(IConverter),
    new SynchronizedConverter(new PdfTools()));

builder.Services.AddScoped<PdfService>();

builder.Services.AddTransient<IEmailSender, EmailService>();

builder.Services.AddScoped<EmailService>();


// ================= IDENTITY =================

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // KHÔNG cần confirm email
    options.SignIn.RequireConfirmedAccount = false;

    // Password đơn giản
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // KHÔNG khóa tài khoản
    options.Lockout.AllowedForNewUsers = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();


// ================= COOKIE =================

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";

    options.AccessDeniedPath = "/Account/Login";
});


// ================= MVC =================

builder.Services.AddControllersWithViews();

builder.Services.AddSession();

var app = builder.Build();


// ================= SESSION =================

app.UseSession();


// ================= ROLE + ADMIN =================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager =
        services.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager =
        services.GetRequiredService<UserManager<ApplicationUser>>();

    // ================= ROLE =================

    string[] roles =
    {
        "Admin",
        "Sale",
        "Accountant",
        "Customer"
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(
                new IdentityRole(role)
            );
        }
    }

    // ================= ADMIN ACCOUNT =================

    string adminEmail = "nanhdung840@gmail.com";

    string adminPassword = "110506";

    var adminUser =
        await userManager.FindByEmailAsync(adminEmail);// Nếu chưa có thì tạo
    if (adminUser == null)
    {
        var newAdmin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FullName = "Toyota Admin",
            Address = "Toyota Enterprise"
        };

        var createAdmin =
            await userManager.CreateAsync(
                newAdmin,
                adminPassword
            );

        if (createAdmin.Succeeded)
        {
            await userManager.AddToRoleAsync(
                newAdmin,
                "Admin"
            );
        }
    }
    else
    {
        // Reset password admin mỗi lần chạy
        var token =
            await userManager.GeneratePasswordResetTokenAsync(adminUser);

        await userManager.ResetPasswordAsync(
            adminUser,
            token,
            adminPassword
        );

        // Reset lock
        adminUser.LockoutEnd = null;
        adminUser.AccessFailedCount = 0;

        await userManager.UpdateAsync(adminUser);

        // Add role Admin nếu chưa có
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(
                adminUser,
                "Admin"
            );
        }
    }
}


// ================= MIDDLEWARE =================

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


// ================= ROUTE =================

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "carDetails",
    pattern: "cars/{slug}",
    defaults: new
    {
        controller = "Car",
        action = "Details"
    }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();


// ================= DATABASE =================

using (var scope = app.Services.CreateScope())
{
    var db =
        scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    db.Database.EnsureCreated();
}

app.Run();