INSERT INTO LOC_Product(ProductName, ProductPrice, ProductCode, Description, UserID) 
VALUES ('Product A', 19.99, 'A001', 'Description for Product A', 1);

INSERT INTO LOC_Product (ProductName, ProductPrice, ProductCode, Description, UserID) 
VALUES ('Product B', 29.99, 'B002', 'Description for Product B', 2);

INSERT INTO LOC_Product (ProductName, ProductPrice, ProductCode, Description, UserID) 
VALUES ('Product C', 39.99, 'C003', 'Description for Product C', 3);

select * from LOC_Product



INSERT INTO [dbo].[LOC_User] (UserName, Email, Password, MobileNo, Address, IsActive)
VALUES ('JohnDoe', 'john.doe@example.com', 'password123', '1234567890', '123 Main St, City, Country', 1);

INSERT INTO [dbo].[LOC_User] (UserName, Email, Password, MobileNo, Address, IsActive)
VALUES ('JaneSmith', 'jane.smith@example.com', 'password456', '0987654321', '456 Elm St, City, Country', 1);

INSERT INTO [dbo].[LOC_User] (UserName, Email, Password, MobileNo, Address, IsActive)
VALUES ('MikeBrown', 'mike.brown@example.com', 'password789', '1122334455', '789 Oak St, City, Country', 0);

select * from LOC_User



INSERT INTO LOC_Order(OrderNumber,OrderDate, CustomerID, PaymentMode, TotalAmount, ShippingAddress, UserID)
VALUES (1,getdate(), 1, 'Credit Card', 150.75, '123 Elm Street', 1);

INSERT INTO LOC_Order (OrderNumber,OrderDate, CustomerID, PaymentMode, TotalAmount, ShippingAddress, UserID)
VALUES (2,getdate(), 2, 'PayPal', 200.00, '456 Oak Avenue', 2);

INSERT INTO LOC_Order (OrderNumber,OrderDate, CustomerID, PaymentMode, TotalAmount, ShippingAddress, UserID)
VALUES (3,getdate(), 3, 'PayPal', 300.00, '789 Pine Road', 3);

select * from LOC_Order 



INSERT INTO LOC_OrderDetail (OrderID, ProductID, Quantity, Amount, TotalAmount, UserID)
VALUES (1, 1, 2, 50.00, 100.00, 1);

INSERT INTO LOC_OrderDetail (OrderID, ProductID, Quantity, Amount, TotalAmount, UserID)
VALUES (2, 2, 6, 70.00, 300.00, 2);

INSERT INTO LOC_OrderDetail (OrderID, ProductID, Quantity, Amount, TotalAmount, UserID)
VALUES (3, 3, 9, 40.00, 400.00, 3);

select * from LOC_OrderDetail 



INSERT INTO LOC_Bills (BillNumber, BillDate, OrderID, TotalAmount, Discount, NetAmount, UserID)
VALUES ('B123456', getdate(), 1, 500.00, 50.00, 450.00, 1);

INSERT INTO LOC_Bills (BillNumber, BillDate, OrderID, TotalAmount, Discount, NetAmount, UserID)
VALUES ('B123457', getdate(), 2, 600.00, 60.00, 500.00, 2);

INSERT INTO LOC_Bills (BillNumber, BillDate, OrderID, TotalAmount, Discount, NetAmount, UserID)
VALUES ('B123458', getdate(), 3, 700.00, 70.00, 550.00, 3);

select * from LOC_Bills 



INSERT INTO LOC_Customer (CustomerName, HomeAddress, Email, MobileNo, GSTNO, CityName, PinCode, NetAmount, UserID)
VALUES ('John Doe', '123 Maple Street', 'john.doe@example.com', '1234567890', 'GST1234567A', 'Springfield', '123456', 1500.75, 1);

INSERT INTO LOC_Customer (CustomerName, HomeAddress, Email, MobileNo, GSTNO, CityName, PinCode, NetAmount, UserID)
VALUES ('Jane Smith', '456 Oak Avenue', 'jane.smith@example.com', '0987654321', 'GST7654321B', 'Shelbyville', '654321', 2000.50, 2);

INSERT INTO LOC_Customer (CustomerName, HomeAddress, Email, MobileNo, GSTNO, CityName, PinCode, NetAmount, UserID)
VALUES ('Alice Johnson', '789 Pine Road', 'alice.johnson@example.com', '1122334455', 'GST1122334C', 'Capital City', '789012', 1750.25, 3);

select * from LOC_Customer 



