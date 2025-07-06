using Microsoft.EntityFrameworkCore;
using Lab2_DbContextSetup.Models;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=RetailInventoryLab2Db;Trusted_Connection=True;Encrypt=False;");
    }
}
