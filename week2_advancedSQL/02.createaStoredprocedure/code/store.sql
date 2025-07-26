IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EmployeeDB')
BEGIN
    CREATE DATABASE EmployeeDB;
END;
GO

USE EmployeeDB;
GO

IF OBJECT_ID('Employees', 'U') IS NOT NULL
    DROP TABLE Employees;
GO

-- Create Employees table
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    DepartmentID INT,
    Salary DECIMAL(10, 2),
    JoinDate DATE
);
GO
INSERT INTO Employees (FirstName, LastName, DepartmentID, Salary, JoinDate)
VALUES 
('Gowri', 'Nandhini', 1, 60000, '2023-06-01'),
('Kiran', 'Kumar', 2, 55000, '2022-04-15'),
('Sita', 'Rani', 1, 58000, '2021-12-20');
GO

-- Drop procedure sp_InsertEmployee if exists
IF OBJECT_ID('sp_InsertEmployee', 'P') IS NOT NULL
    DROP PROCEDURE sp_InsertEmployee;
GO

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
END;
GO

IF OBJECT_ID('sp_GetEmployeesByDepartment', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetEmployeesByDepartment;
GO
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
END;
GO
EXEC sp_GetEmployeesByDepartment @DepartmentID = 1;
GO
EXEC sp_InsertEmployee 
    @FirstName = 'Arjun',
    @LastName = 'Reddy',
    @DepartmentID = 3,
    @Salary = 67000,
    @JoinDate = '2024-01-10';
GO
SELECT * FROM Employees;
GO