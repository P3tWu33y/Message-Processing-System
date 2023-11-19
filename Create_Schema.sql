-- Create Database if not exists
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'main')
BEGIN
    CREATE DATABASE main;
END
GO

-- Use the main database
USE main;
GO

-- Create messages table with 'id' as an identity column and 'isProcessed' defaulting to 0
CREATE TABLE dbo.messages (
    id INT IDENTITY(1,1) PRIMARY KEY,
    messages INT NOT NULL,
    isProcessed BIT DEFAULT 0
);
GO
