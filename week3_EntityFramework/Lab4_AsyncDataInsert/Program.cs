using Lab4_AsyncDataInsert.Models;

class Program
{
    static async Task Main()
    {
        using var context = new AppDbContext();

        if (!context.Categories.Any())
        {
            var electronics = new Category { Name = "Electronics" };
            var groceries = new Category { Name = "Groceries" };

            await context.Categories.AddRangeAsync(electronics, groceries);

            var product1 = new Product { Name = "Laptop", Price = 75000, Category = electronics };
            var product2 = new Product { Name = "Rice Bag", Price = 1200, Category = groceries };

            await context.Products.AddRangeAsync(product1, product2);
            await context.SaveChangesAsync();

            Console.WriteLine("✅ Data inserted successfully using async methods.");
        }

        var products = context.Products.Select(p => new
        {
            p.Id,
            p.Name,
            p.Price,
            Category = p.Category.Name
        }).ToList();

        Console.WriteLine("\n📦 Product List:");
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("ID\tName\t\tPrice\t\tCategory");
        Console.WriteLine("-----------------------------------------");

        foreach (var p in products)
            Console.WriteLine($"{p.Id}\t{p.Name,-15}\t₹{p.Price}\t{p.Category}");

        Console.WriteLine("-----------------------------------------");
    }
}
