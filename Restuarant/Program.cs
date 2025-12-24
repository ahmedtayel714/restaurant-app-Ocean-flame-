using Microsoft.EntityFrameworkCore;
using RestaurantApp;
using System.Collections.Generic;
using Restuarant.Data;
using Restuarant; // For OrderItem in some projects
using RestaurantApp; // For MenuItem, Order in some projects

var builder = WebApplication.CreateBuilder(args);

// Add SQLite database
builder.Services.AddDbContext<RestaurantContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add session support for admin authentication
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<Restuarant.Core.Interfaces.IUnitOfWork, Restuarant.Data.UnitOfWork>();
builder.Services.AddScoped<RestaurantApp.Services.Reporting.ReportingService>();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RestaurantContext>();
    context.Database.EnsureCreated();
    
    // Seed menu items if empty
    if (!context.MenuItems.Any())
    {
        context.MenuItems.AddRange(
            new MenuItem { Name = "Calamari", Description = "Fresh calamari", Price = 155, StockQuantity = 20, ImageUrl = "img/menu/lobster-bisque.jpg", Category = "Seafood" },
            new MenuItem { Name = "Bread", Description = "Fresh bread", Price = 5, StockQuantity = 20, ImageUrl = "img/menu/bread-barrel.jpg", Category = "Pastries" },
            new MenuItem { Name = "Meat Casserole", Description = "Delicious meat casserole", Price = 85, StockQuantity = 20, ImageUrl = "img/menu/caesar.jpg", Category = "Casseroles" },
            new MenuItem { Name = "Pasta Casserole", Description = "Pasta casserole", Price = 55, StockQuantity = 20, ImageUrl = "img/menu/greek-salad.jpg", Category = "Casseroles" },
            new MenuItem { Name = "Shrimp", Description = "Fresh shrimp", Price = 175, StockQuantity = 20, ImageUrl = "img/menu/lobster-roll.jpg", Category = "Seafood" },
            new MenuItem { Name = "Seafood Soup", Description = "Seafood soup", Price = 70, StockQuantity = 20, ImageUrl = "img/menu/seafood.jpeg", Category = "Seafood" },
            new MenuItem { Name = "Pizza", Description = "Delicious pizza", Price = 95, StockQuantity = 20, ImageUrl = "img/menu/mozzarella.jpg", Category = "Pastries" },
            new MenuItem { Name = "Crepe", Description = "Sweet or savory crepe", Price = 85, StockQuantity = 20, ImageUrl = "img/menu/spinach-salad.jpg", Category = "Sandwiches" },
            new MenuItem { Name = "Shawarma", Description = "Chicken shawarma", Price = 55, StockQuantity = 20, ImageUrl = "img/menu/tuscan-grilled.jpg", Category = "Sandwiches" },
            new MenuItem { Name = "Burger", Description = "Juicy burger", Price = 90, StockQuantity = 20, ImageUrl = "img/menu/burger img.jpg", Category = "Sandwiches" }
        );
        context.SaveChanges();
    }

    // Seed Orders for Reporting Test
    if (!context.Orders.Any())
    {
        var items = context.MenuItems.ToList();
        if (items.Any())
        {
            var order1 = new Order
            {
                CreatedAt = DateTime.Now,
                Status = "Completed",
                TotalPrice = items[0].Price * 2 + items[1].Price, 
                Items = new List<OrderItem>
                {
                    new OrderItem { Name = items[0].Name, Quantity = 2, Total = items[0].Price * 2 },
                    new OrderItem { Name = items[1].Name, Quantity = 1, Total = items[1].Price }
                }
            };

            var order2 = new Order
            {
                CreatedAt = DateTime.Now,
                Status = "Completed",
                TotalPrice = items[2].Price,
                Items = new List<OrderItem>
                {
                    new OrderItem { Name = items[2].Name, Quantity = 1, Total = items[2].Price }
                }
            };
            
            context.Orders.AddRange(order1, order2);
            context.SaveChanges();
        }
    }

    // Seed SystemSettings if empty
    if (!context.Settings.Any())
    {
        context.Settings.Add(new SystemSettings 
        { 
            TaxRate = 14.0m, 
            Currency = "EGP", 
            IsOpen = true 
        });
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Enable session

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.Run("http://localhost:5000");