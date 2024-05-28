using E_commerceElect.Models;
using E_commerceElect.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ElectDBConnection")));
builder.Services.AddScoped<IRepositoryA<Article>, SqlProductRepository>();
builder.Services.AddScoped<IRepository<Categorie>, SqlCategorieRepository>();
builder.Services.AddScoped<IRepository<LigneCommande>, SqlLigneCommandeRepository>();
builder.Services.AddScoped<IRepository<LignePanier>, SqlLignePanierRepository>();
builder.Services.AddScoped<IRepositoryP<Panier>, SqlPanierRepository>();
builder.Services.AddScoped<IRepositoryC<Commande>, SqlCommandeRepository>();
builder.Services.AddScoped<IRepository<ApplicationUser>, SqlUserRepository>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

// Add Stripe configuration
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
