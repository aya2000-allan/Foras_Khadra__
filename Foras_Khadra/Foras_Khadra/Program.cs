using Foras_Khadra.Data;
using Foras_Khadra.Models;
using Foras_Khadra.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ===== إعداد قاعدة البيانات =====
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ===== إعداد Identity =====
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ===== إعداد الكوكيز =====
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Organization/Login";
    options.AccessDeniedPath = "/Organization/Login";
});

// ===== إعداد خدمات البريد =====
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailService, MailService>();

// ===== Session & Cache =====
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ===== إنشاء الأدوار وحساب الأدمن وربط المنظمات =====
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // ---- إنشاء الأدوار لو مش موجودة ----
    string[] roles = { "Admin", "User", "Organization" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // ---- إنشاء حساب الأدمن الثابت ----
    string adminEmail = "admin@org.com";
    string adminPassword = "Admin@123";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FullName = "System Admin",
            CreatedAt = DateTime.Now
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    // ---- ربط كل المنظمات بحساباتهم بدون التأثير على Admin أو User ----
    var orgRoleId = context.Roles.FirstOrDefault(r => r.Name == "Organization")?.Id;

    if (orgRoleId != null)
    {
        // كل UserIds للمنظمات
        var orgUserIds = context.UserRoles
            .Where(ur => ur.RoleId == orgRoleId)
            .Select(ur => ur.UserId)
            .ToList();

        var orgUsers = context.Users
            .Where(u => orgUserIds.Contains(u.Id))
            .ToList();

        // كل Organizations بدون UserId
        var organizations = context.Organizations
            .Where(o => o.UserId == null)
            .ToList();

        foreach (var org in organizations)
        {
            var user = orgUsers.FirstOrDefault(u => u.Email.ToLower() == org.ContactEmail.ToLower());
            if (user != null)
                org.UserId = user.Id;
        }

        context.SaveChanges();
    }
}

// ===== Configure the HTTP request pipeline =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
