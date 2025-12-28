using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Foras_Khadra.Services; // ← تأكد أنك أضفت هذا الـ using
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, EmailSender>();

// تسجيل إعدادات وإيميل التواصل
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailService, MailService>();

// 👇 تسجيل خدمة IMailService
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings")); // تأكد من وجود MailSettings بالقيم

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Organization"))
    {
        await roleManager.CreateAsync(new IdentityRole("Organization"));
    }

    // المستخدم الأول
    string email = "test@org.com";
    string password = "Test123!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = "Test Org",
            Role = UserRole.Organization,
            Language = "en",
            CreatedAt = DateTime.Now
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Organization");
        }
    }

    // المستخدم الثاني
    string email2 = "tojan050@gmail.com";
    string password2 = "YourSecurePassword!";

    if (await userManager.FindByEmailAsync(email2) == null)
    {
        var user2 = new ApplicationUser
        {
            UserName = email2,
            Email = email2,
            FullName = "Tojan AboGhola",
            Role = UserRole.User,
            Language = "en",
            CreatedAt = DateTime.Now
        };

        var result2 = await userManager.CreateAsync(user2, password2);
        if (result2.Succeeded)
        {
            await userManager.AddToRoleAsync(user2, "Organization");
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
