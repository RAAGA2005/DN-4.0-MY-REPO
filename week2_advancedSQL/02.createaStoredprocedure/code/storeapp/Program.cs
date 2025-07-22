using System;
using System.Data;
using Microsoft.Data.SqlClient; // ✅ Updated namespace

class Program
{
    static void Main()
    {
        string masterConnStr = "Server=localhost;Database=master;Trusted_Connection=True;";
        string employeeDbConnStr = "Server=localhost;Database=EmployeeDB;Trusted_Connection=True;";

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
                Console.WriteLine("Database checked/created.");
            }
        }

        // Step 2: Create table and insert data
        using (SqlConnection conn = new SqlConnection(employeeDbConnStr))
        {
            conn.Open();

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
                );

                INSERT INTO Employees (FirstName, LastName, DepartmentID, Salary, JoinDate)
                VALUES 
                ('Niharika', 'Raaga', 1, 60000, '2023-06-01'),
                ('sohan', 'Raaga', 2, 55000, '2022-04-15'),
                ('Hasini', 'Raaga', 1, 58000, '2021-12-20');";
            
            using (SqlCommand cmd = new SqlCommand(setupQuery, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Table created and initial data inserted.");
            }

            // Step 3: Create sp_InsertEmployee
            string createInsertProc = @"
                IF OBJECT_ID('sp_InsertEmployee', 'P') IS NOT NULL
                    DROP PROCEDURE sp_InsertEmployee;

                CREATE PROCEDURE sp_InsertEmployee
                    @FirstName VARCHAR(50),
                    @LastName VARCHAR(50),
                    @DepartmentID INT,
                    @Salary DECIMAL(10,2),
                    @JoinDate DATE
                AS
                BEGIN
                    INSERT INTO Employees (FirstName, LastName, DepartmentID, Salary, JoinDate)
                    VALUES (@FirstName, @LastName, @DepartmentID, @Salary, @JoinDate);
                END;";
            using (SqlCommand cmd = new SqlCommand(createInsertProc, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Procedure sp_InsertEmployee created.");
            }

            // Step 4: Create sp_GetEmployeesByDepartment
            string createSelectProc = @"
                IF OBJECT_ID('sp_GetEmployeesByDepartment', 'P') IS NOT NULL
                    DROP PROCEDURE sp_GetEmployeesByDepartment;

                CREATE PROCEDURE sp_GetEmployeesByDepartment
                    @DepartmentID INT
                AS
                BEGIN
                    SELECT 
                        EmployeeID,
                        FirstName,
                        LastName,
                        DepartmentID,
                        Salary,
                        JoinDate
                    FROM Employees
                    WHERE DepartmentID = @DepartmentID;
                END;";
            using (SqlCommand cmd = new SqlCommand(createSelectProc, conn))
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("Procedure sp_GetEmployeesByDepartment created.");
            }

            // Step 5: Execute sp_InsertEmployee
            using (SqlCommand cmd = new SqlCommand("sp_InsertEmployee", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", "Dharani");
                cmd.Parameters.AddWithValue("@LastName", "Raaga");
                cmd.Parameters.AddWithValue("@DepartmentID", 3);
                cmd.Parameters.AddWithValue("@Salary", 67000);
                cmd.Parameters.AddWithValue("@JoinDate", DateTime.Parse("2024-01-10"));

                cmd.ExecuteNonQuery();
                Console.WriteLine("Inserted new employee using sp_InsertEmployee.");
            }

            // Step 6: Execute sp_GetEmployeesByDepartment
            using (SqlCommand cmd = new SqlCommand("sp_GetEmployeesByDepartment", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentID", 1);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\nEmployees in Department 1:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["EmployeeID"]}, {reader["FirstName"]} {reader["LastName"]}, {reader["Salary"]}, Joined: {reader["JoinDate"]}");
                    }
                }
            }

            // Step 7: Display all Employees
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\nAll Employees:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["EmployeeID"]}, {reader["FirstName"]} {reader["LastName"]}, Dept: {reader["DepartmentID"]}, Salary: {reader["Salary"]}, Joined: {reader["JoinDate"]}");
                    }
                }
            }
        }
    }
}
