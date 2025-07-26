using Microsoft.EntityFrameworkCore;
using Lab3_EFCoreMigration.Models;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=RetailInventory_Lab3;Trusted_Connection=True;Encrypt=False;");
    }
}
