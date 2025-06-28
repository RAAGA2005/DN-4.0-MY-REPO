using System;
using Microsoft.Data.SqlClient;
using System.Data;

class Program
{
    static void Main()
    {
        string serverName = "localhost\\SQLEXPRESS"; // Change if your instance is different
        string masterConnStr = $"Server={serverName};Database=master;Trusted_Connection=True;";
        string employeeDbConnStr = $"Server={serverName};Database=EmployeeDB;Trusted_Connection=True;";

        // Step 1: Create EmployeeDB if not exists
        using (SqlConnection conn = new SqlConnection(masterConnStr))
        {
            conn.Open();
            string createDbQuery = @"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EmployeeDB')
                BEGIN
                    CREATE DATABASE EmployeeDB;
                END;";
            using (SqlCommand cmd = new SqlCommand(createDbQuery, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("✅ Database checked/created.");
            }
        }

        // Step 2: Connect to EmployeeDB and perform operations
        using (SqlConnection conn = new SqlConnection(employeeDbConnStr))
        {
            conn.Open();

            // Step 3 & 4: Drop and Create Table
            string setupTableQuery = @"
                IF OBJECT_ID('Employees', 'U') IS NOT NULL
                    DROP TABLE Employees;

                CREATE TABLE Employees (
                    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
                    FirstName VARCHAR(50),
                    LastName VARCHAR(50),
                    DepartmentID INT,
                    Salary DECIMAL(10, 2),
                    JoinDate DATE
                );";
            using (SqlCommand cmd = new SqlCommand(setupTableQuery, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("✅ Employees table created.");
            }

            // Step 5: Insert sample data
            string insertQuery = @"
                INSERT INTO Employees (FirstName, LastName, DepartmentID, Salary, JoinDate)
                VALUES
                ('Niharika', 'Raaga', 1, 60000, '2023-06-01'),
                ('Sohan', 'Raaga', 2, 55000, '2022-04-15'),
                ('Hasini', 'Raaga', 1, 58000, '2021-12-20'),
                ('dharani', 'chinna', 3, 67000, '2024-01-10');";
            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("✅ Sample employee data inserted.");
            }

            // Step 6 & 7: Drop and Create stored procedure
            string createProcQuery = @"
                IF OBJECT_ID('sp_GetEmployeeCountByDepartment', 'P') IS NOT NULL
                    DROP PROCEDURE sp_GetEmployeeCountByDepartment;

                CREATE PROCEDURE sp_GetEmployeeCountByDepartment
                    @DepartmentID INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT COUNT(*) AS TotalEmployees
                    FROM Employees
                    WHERE DepartmentID = @DepartmentID;
                END;";
            using (SqlCommand cmd = new SqlCommand(createProcQuery, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("✅ Stored procedure created.");
            }

            // Step 8: Execute the stored procedure
            using (SqlCommand cmd = new SqlCommand("sp_GetEmployeeCountByDepartment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentID", 1);

                int count = (int)cmd.ExecuteScalar();
                Console.WriteLine($"\n📊 Total employees in Department 1: {count}");
            }
        }
    }
}
