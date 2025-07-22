using System;
using Lab2_DbContextSetup.Models;

class Program
{
    static void Main()
    {
        using var context = new AppDbContext();

        Console.WriteLine("Database context setup successful! ✔️");
    }
}
