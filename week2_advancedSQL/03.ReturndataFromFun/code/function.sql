-- Step 1: Create the database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EmployeeDB')
BEGIN
    CREATE DATABASE EmployeeDB;
END;
GO

-- Step 2: Use the database
USE EmployeeDB;
GO

-- Step 3: Drop the table if it already exists
IF OBJECT_ID('Employees', 'U') IS NOT NULL
    DROP TABLE Employees;
GO

-- Step 4: Create Employees table
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    DepartmentID INT,
    Salary DECIMAL(10, 2),
    JoinDate DATE
);
GO

-- Step 5: Insert sample data
INSERT INTO Employees (FirstName, LastName, DepartmentID, Salary, JoinDate)
VALUES
('Niharika', 'Raaga', 1, 60000, '2023-06-01'), 
('Sohan', 'Raaga', 2, 55000, '2022-04-15'), 
('Hasini', 'Raaga', 1, 58000, '2021-12-20'), 
('Dharani', 'Chinna', 3, 67000, '2024-01-10');
GO

-- Step 6: Drop the procedure if it already exists
IF OBJECT_ID('sp_GetEmployeeCountByDepartment', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetEmployeeCountByDepartment;
GO

-- Step 7: Create stored procedure to count employees in a department
CREATE PROCEDURE sp_GetEmployeeCountByDepartment
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        COUNT(*) AS TotalEmployees
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO

-- Step 8: Execute the procedure for DepartmentID = 1
EXEC sp_GetEmployeeCountByDepartment @DepartmentID = 1;
GO
