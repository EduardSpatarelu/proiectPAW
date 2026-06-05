using BARmonica.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BARmonica.Web.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adding the connection to database.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Setăm reguli mai simple pentru parole
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// "Angajam" Repository-ul si Serviciul pentru Categorii
builder.Services.AddScoped<BARmonica.Web.Repositories.ICategoryRepository, BARmonica.Web.Repositories.CategoryRepository>();
builder.Services.AddScoped<BARmonica.Web.Services.ICategoryService, BARmonica.Web.Services.CategoryService>();

builder.Services.AddScoped<BARmonica.Web.Repositories.IProducerRepository, BARmonica.Web.Repositories.ProducerRepository>();
builder.Services.AddScoped<BARmonica.Web.Services.IProducerService, BARmonica.Web.Services.ProducerService>();

builder.Services.AddScoped<BARmonica.Web.Repositories.IProductRepository, BARmonica.Web.Repositories.ProductRepository>();
builder.Services.AddScoped<BARmonica.Web.Services.IProductService, BARmonica.Web.Services.ProductService>();

builder.Services.AddScoped<BARmonica.Web.Services.IAuthService, BARmonica.Web.Services.AuthService>();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// --- SCRIPT PENTRU CREAREA ROLURILOR (ADMIN SI CLIENT) ---
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<BARmonica.Web.Models.ApplicationUser>>();

    // 1. Creăm rolurile în baza de date dacă nu există deja
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole("Admin"));

    if (!await roleManager.RoleExistsAsync("Client"))
        await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole("Client"));

    // 2. Îi dăm contului tău rolul de Admin (modifică email-ul dacă ai folosit altul)
    var adminUser = await userManager.FindByEmailAsync("eduard@barmonica.ro");
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

app.Run();
