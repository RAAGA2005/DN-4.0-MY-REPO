using Microsoft.EntityFrameworkCore;
using Lab4_AsyncDataInsert.Models;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=RetailInventory_Lab4;Trusted_Connection=True;Encrypt=False;");
    }
}
