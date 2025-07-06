using Microsoft.EntityFrameworkCore;
using Models;
var context = new AppDbContext();
await context.Database.EnsureCreatedAsync();
if (!context.Products.Any())
{
    var category = new Category { Name = "Electronics" };
    var products = new List<Product>
    {
        new Product { Name = "Laptop", Price = 75000, Category = category },
        new Product { Name = "Smartphone", Price = 35000, Category = category },
        new Product { Name = "TV", Price = 55000, Category = category },
    };

    await context.Products.AddRangeAsync(products);
    await context.SaveChangesAsync();
    Console.WriteLine(" Sample products inserted.");
}
var productsList = await context.Products.ToListAsync();
Console.WriteLine(" All Products:");
foreach (var p in productsList)
    Console.WriteLine($"{p.Name} - ₹{p.Price}");
var productById = await context.Products.FindAsync(1);
Console.WriteLine($" Found: {productById?.Name}");
var expensive = await context.Products.FirstOrDefaultAsync(p => p.Price > 50000);
Console.WriteLine($" Expensive: {expensive?.Name}");
