using Lab3_EFCoreMigration.Models;

class Program
{
    static void Main()
    {
        using var context = new AppDbContext();

        if (!context.Categories.Any())
        {
            var category = new Category { Name = "Electronics" };
            var product1 = new Product { Name = "Laptop", Price = 49999, Category = category };
            var product2 = new Product { Name = "Headphones", Price = 2999, Category = category };

            context.Categories.Add(category);
            context.Products.AddRange(product1, product2);

            context.SaveChanges();
            Console.WriteLine("✅ Sample data inserted.");
        }

        var products = context.Products.Select(p => new {
            p.Id, p.Name, p.Price, Category = p.Category.Name
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
