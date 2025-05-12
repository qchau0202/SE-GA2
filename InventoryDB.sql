CREATE DATABASE InventoryDB
USE InventoryDB;

/* Creating the Item table */
CREATE TABLE Item (
    ItemID INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(100) NOT NULL,
    Size NVARCHAR(50),
    UnitPrice DECIMAL(10,2)
);

/* Creating the Agent table */
CREATE TABLE Agent (
    AgentID INT PRIMARY KEY IDENTITY(1,1),
    AgentName NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200)
);

/* Creating the Users table */
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    LockStatus BIT DEFAULT 0
);

/* Creating the Order table */
CREATE TABLE [Order] (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderDate DATE NOT NULL,
    AgentID INT,
    FOREIGN KEY (AgentID) REFERENCES Agent(AgentID)
);

/* Creating the OrderDetail table */
CREATE TABLE OrderDetail (
    ID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT,
    ItemID INT,
    Quantity INT NOT NULL,
    UnitAmount DECIMAL(10,2),
    FOREIGN KEY (OrderID) REFERENCES [Order](OrderID),
    FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
);

/* We added this for more experience */
CREATE TABLE Cart (
    CartID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    ItemID INT NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
);

/* Inserting data into Item table */
INSERT INTO Item (ItemName, Size, UnitPrice) VALUES
('Laptop', '15 inch', 999.99),
('Smartphone', '6 inch', 599.99),
('Tablet', '10 inch', 399.99),
('Headphones', 'Over-ear', 89.99),
('Mouse', 'Standard', 29.99),
('Keyboard', 'Mechanical', 79.99),
('Monitor', '24 inch', 199.99),
('Printer', 'Laser', 149.99),
('Webcam', 'HD', 59.99),
('Speaker', 'Bluetooth', 69.99),
('Router', 'WiFi 6', 129.99),
('External HDD', '1TB', 89.99),
('USB Drive', '64GB', 19.99),
('Smart Watch', 'Standard', 199.99),
('Camera', 'DSLR', 499.99),
('Microphone', 'USB', 79.99),
('Projector', '1080p', 299.99),
('VR Headset', 'Standard', 399.99),
('Game Console', 'NextGen', 499.99),
('Smart Bulb', 'RGB', 29.99);

/* Inserting data into Agent table */
INSERT INTO Agent (AgentName, Address) VALUES
('Tech Distributors', '123 Tech St, Boston, MA'),
('Gadget World', '456 Gadget Rd, New York, NY'),
('Electro Mart', '789 Electro Ave, Chicago, IL'),
('TechTrend', '101 Trend Blvd, Los Angeles, CA'),
('Gizmo Galaxy', '202 Gizmo Ln, Houston, TX'),
('Circuit City', '303 Circuit Dr, Miami, FL'),
('TechTrove', '404 Tech Way, Seattle, WA'),
('Gadget Gurus', '505 Guru St, San Francisco, CA'),
('Electro Elite', '606 Elite Rd, Dallas, TX'),
('Tech Titans', '707 Titan Ave, Atlanta, GA'),
('Gizmo Grid', '808 Grid Blvd, Phoenix, AZ'),
('Circuit Central', '909 Central Ln, Denver, CO'),
('Tech Tempo', '1010 Tempo Dr, Portland, OR'),
('Gadget Grove', '1111 Grove St, Austin, TX'),
('Electro Edge', '1212 Edge Rd, Charlotte, NC');

/* Inserting data into Users table */
INSERT INTO Users (UserName, Email, Password, LockStatus) VALUES
('john_doe', 'john.doe@email.com', 'Pass123!', 0),
('jane_smith', 'jane.smith@email.com', 'Secure456!', 0),
('bob_jones', 'bob.jones@email.com', 'MyPass789!', 0),
('admin', 'admin@gmail.com', 'admin123', 0)

/* Inserting data into Order table */
INSERT INTO [Order] (OrderDate, AgentID) VALUES
('2025-01-10', 1),
('2025-01-15', 2),
('2025-01-20', 3),
('2025-02-01', 4),
('2025-02-05', 5),
('2025-02-10', 6),
('2025-02-15', 7),
('2025-03-01', 8),
('2025-03-05', 9),
('2025-03-10', 10),
('2025-03-15', 11),
('2025-04-01', 12),
('2025-04-05', 13),
('2025-04-10', 14),
('2025-04-15', 15),
('2025-05-01', 1),
('2025-05-02', 2),
('2025-05-03', 3),
('2025-05-04', 4),
('2025-05-05', 5);

/* Inserting data into OrderDetail table */
INSERT INTO OrderDetail (OrderID, ItemID, Quantity, UnitAmount) VALUES
(1, 1, 5, 999.99),
(1, 2, 10, 599.99),
(2, 3, 8, 399.99),
(2, 4, 15, 89.99),
(3, 5, 20, 29.99),
(3, 6, 12, 79.99),
(4, 7, 6, 199.99),
(4, 8, 4, 149.99),
(5, 9, 10, 59.99),
(5, 10, 8, 69.99),
(6, 11, 7, 129.99),
(6, 12, 5, 89.99),
(7, 13, 25, 19.99),
(7, 14, 3, 199.99),
(8, 15, 2, 499.99),
(9, 16, 9, 79.99),
(10, 17, 4, 299.99),
(11, 18, 3, 399.99),
(12, 19, 2, 499.99),
(13, 20, 15, 29.99),
(14, 1, 6, 999.99),
(15, 2, 7, 599.99),
(16, 3, 5, 399.99),
(17, 4, 10, 89.99),
(18, 5, 12, 29.99);

ALTER TABLE [Order]
ADD Status NVARCHAR(50) NOT NULL DEFAULT 'Pending';

ALTER TABLE [Order]
ADD UserID INT NULL;

ALTER TABLE [Order]
ADD CONSTRAINT FK_Order_Users FOREIGN KEY (UserID) REFERENCES Users(UserID);

SELECT * FROM Agent
SELECT * FROM Item
SELECT * FROM [Order]
SELECT * FROM OrderDetail
SELECT * FROM Users

DROP TABLE Agent
DROP TABLE Item
DROP TABLE [ORDER]
DROP TABLE OrderDetail
DROP TABLE Users