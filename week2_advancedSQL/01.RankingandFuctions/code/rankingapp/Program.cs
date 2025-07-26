using System;
using System.Data;
using System.Data.SQLite;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=products.db;Version=3;";

        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Create the Products table
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Products (
                    ProductID INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductName TEXT,
                    Category TEXT,
                    Price REAL
                );";
            new SQLiteCommand(createTableQuery, connection).ExecuteNonQuery();

            // Clear the table
            string deleteQuery = "DELETE FROM Products;";
            new SQLiteCommand(deleteQuery, connection).ExecuteNonQuery();

            // Insert data
            string insertQuery = @"
                INSERT INTO Products (ProductName, Category, Price) VALUES
                ('Laptop', 'Electronics', 900),
                ('Smartphone', 'Electronics', 700),
                ('Headphones', 'Electronics', 200),
                ('TV', 'Electronics', 1200),
                ('Microwave', 'Home Appliances', 400),
                ('Blender', 'Home Appliances', 150),
                ('Refrigerator', 'Home Appliances', 1000),
                ('Oven', 'Home Appliances', 800),
                ('Shampoo', 'Personal Care', 100),
                ('Perfume', 'Personal Care', 300),
                ('Cream', 'Personal Care', 250),
                ('Lotion', 'Personal Care', 150);";
            new SQLiteCommand(insertQuery, connection).ExecuteNonQuery();

            // Window function query
            string rankedQuery = @"
                WITH RankedProducts AS (
                    SELECT 
                        ProductName,
                        Category,
                        Price,
                        ROW_NUMBER() OVER (PARTITION BY Category ORDER BY Price DESC) AS RowNum,
                        RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS RankVal,
                        DENSE_RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS DenseRankVal
                    FROM Products
                )
                SELECT * FROM RankedProducts
                WHERE RowNum <= 3;";

            using (SQLiteCommand command = new SQLiteCommand(rankedQuery, connection))
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("Top 3 Most Expensive Products per Category:\n");

                while (reader.Read())
                {
                    Console.WriteLine($"Product: {reader["ProductName"]}, " +
                                      $"Category: {reader["Category"]}, " +
                                      $"Price: {reader["Price"]}, " +
                                      $"RowNum: {reader["RowNum"]}, " +
                                      $"Rank: {reader["RankVal"]}, " +
                                      $"DenseRank: {reader["DenseRankVal"]}");
                }
            }
        }
    }
}

