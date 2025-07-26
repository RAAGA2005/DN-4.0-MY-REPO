using System;
using System.Collections.Generic;

namespace ECommerceSearch
{
    // Step 1: Define the Product Class
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(int id, string name, decimal price)
        {
            Id = id;
            Name = name.ToLower(); // normalize for search
            Price = price;
        }
    }

    // Step 2: Search Function Using Linear Search
    public class ProductSearch
    {
        private List<Product> products;

        public ProductSearch()
        {
            // Sample Product List
            products = new List<Product>
            {
                new Product(1, "Laptop", 75000),
                new Product(2, "Smartphone", 30000),
                new Product(3, "Headphones", 1500),
                new Product(4, "Smartwatch", 5000),
                new Product(5, "Lamp", 700),
                new Product(6, "USB Cable", 300),
                new Product(7, "Bluetooth Speaker", 2500)
            };
        }

        // Linear Search by Product Name
        public List<Product> SearchByName(string keyword)
        {
            keyword = keyword.ToLower();
            List<Product> results = new List<Product>();

            foreach (var product in products)
            {
                if (product.Name.Contains(keyword))
                {
                    results.Add(product);
                }
            }

            return results;
        }

        // Display the Search Results
        public void Display(List<Product> results)
        {
            if (results.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine("\nSearch Results:");
            foreach (var product in results)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, Price: ₹{product.Price}");
            }
        }
    }

    // Step 3: Main Method (Client)
    class Program
    {
        static void Main(string[] args)
        {
            ProductSearch searchEngine = new ProductSearch();

            Console.Write("Enter search keyword: ");
            string input = Console.ReadLine();

            var results = searchEngine.SearchByName(input);
            searchEngine.Display(results);
        }
    }
}
