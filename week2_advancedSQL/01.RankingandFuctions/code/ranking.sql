-- Check if table exists, then create
IF OBJECT_ID('dbo.Products', 'U') IS NULL
BEGIN
    CREATE TABLE Products (
        ProductID INT IDENTITY(1,1) PRIMARY KEY,
        ProductName NVARCHAR(100),
        Category NVARCHAR(100),
        Price FLOAT
    );
END

-- Clear existing data
DELETE FROM Products;

-- Insert sample data
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
('Lotion', 'Personal Care', 150);

-- Get top 3 expensive products per category
WITH RankedProducts AS (
    SELECT 
        ProductName,
        Category,
        Price,
        ROW_NUMBER() OVER (PARTITION BY Category ORDER BY Price DESC) AS RowNum
    FROM Products
)
SELECT *
FROM RankedProducts
WHERE RowNum <= 3;
