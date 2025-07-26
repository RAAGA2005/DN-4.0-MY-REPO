using System;
using Microsoft.Data.SqlClient;
using System.Data;

class Program
{
    static void Main()
    {
        string serverName = "localhost\\SQLEXPRESS"; // ✅ Change this if needed
        string masterConnStr = $"Server={serverName};Database=master;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
        string employeeDbConnStr = $"Server={serverName};Database=EmployeeDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

        // Step 1: Create EmployeeDB if it doesn't exist
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

        // Step 2: Create table and insert data in EmployeeDB
        using (SqlConnection conn = new SqlConnection(employeeDbConnStr))
        {
            conn.Open();

            // Step 3: Drop and Create Employees table
            string setupQuery = @"
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
            using (SqlCommand cmd = new SqlCommand(setupQuery, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("✅ Employees table created.");
            }

            // Step 4: Insert sample data
            string insertQuery = @"
                INSERT INTO Employees (FirstName, LastName, DepartmentID, Salary, JoinDate)
                VALUES
                ('Niharika', 'Raaga', 1, 60000, '2023-06-01'),
                ('Sohan', 'Raaga', 2, 55000, '2022-04-15'),
                ('Hasini', 'Raaga', 1, 58000, '2021-12-20'),
                ('Dharani', 'Chinna', 3, 67000, '2024-01-10');";
            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("✅ Sample employee data inserted.");
            }

            // Step 5: Drop existing procedure if exists
            string dropProcQuery = @"
                IF OBJECT_ID('sp_GetEmployeeCountByDepartment', 'P') IS NOT NULL
                    DROP PROCEDURE sp_GetEmployeeCountByDepartment;";
            using (SqlCommand cmd = new SqlCommand(dropProcQuery, conn))
            {
                cmd.ExecuteNonQuery();
            }

            // Step 6: Create procedure
            string createProcQuery = @"
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

            // Step 7: Execute procedure for DepartmentID = 1
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
