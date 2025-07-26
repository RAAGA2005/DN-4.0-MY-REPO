using Microsoft.EntityFrameworkCore;
using RetailInventory.Models;

using var db = new AppDbContext();

if (!db.Categories.Any())
{
    var electronics = new Category { Name = "Electronics" };
    var groceries = new Category { Name = "Groceries" };

    db.Categories.AddRange(electronics, groceries);

    db.Products.AddRange(
        new Product { Name = "Laptop", Quantity = 10, Category = electronics },
        new Product { Name = "Headphones", Quantity = 25, Category = electronics },
        new Product { Name = "Apple", Quantity = 50, Category = groceries }
    );

    db.SaveChanges();
}

var products = db.Products.Include(p => p.Category).ToList();

Console.WriteLine($"{"ID",3} {"Name",-15} {"Qty",5} {"Category",-10}");
Console.WriteLine(new string('-', 40));

foreach (var p in products)
{
    Console.WriteLine($"{p.ProductId,3} {p.Name,-15} {p.Quantity,5} {p.Category.Name,-10}");
}
