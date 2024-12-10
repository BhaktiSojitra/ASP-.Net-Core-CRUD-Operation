-- select all
CREATE PROCEDURE  [dbo].[PR_Order_SelectAll]
AS
BEGIN
	SELECT 
		   [dbo].[LOC_Order].[OrderID],
		   [dbo].[LOC_Order].[OrderNumber],
		   [dbo].[LOC_Order].[OrderDate],
		   [dbo].[LOC_Order].[CustomerID],
		   [dbo].[LOC_Order].[PaymentMode],
		   [dbo].[LOC_Order].[TotalAmount],
		   [dbo].[LOC_Order].[ShippingAddress],
		   [dbo].[LOC_Order].[UserID]
	FROM [dbo].[LOC_Order]
	INNER JOIN [dbo].[LOC_Customer] 
	ON [dbo].[LOC_Order].[CustomerID] = [dbo].[LOC_Customer].[CustomerID]
	INNER JOIN [dbo].[LOC_User] 
	ON [dbo].[LOC_Order].[UserID] = [dbo].[LOC_User].[UserID]
	ORDER BY [dbo].[LOC_Order].[OrderDate],
			 [dbo].[LOC_Order].[PaymentMode],
		     [dbo].[LOC_Order].[TotalAmount],
		     [dbo].[LOC_Order].[ShippingAddress];
END

EXEC [dbo].[PR_Order_SelectAll] 


-- by primary key
CREATE PROCEDURE [dbo].[PR_Order_SelectByPK]
	@OrderID int
AS
BEGIN
	SELECT 
		   [dbo].[LOC_Order].[OrderDate],
		   [dbo].[LOC_Order].[OrderNumber],
		   [dbo].[LOC_Order].[CustomerID],
		   [dbo].[LOC_Order].[PaymentMode],
		   [dbo].[LOC_Order].[TotalAmount],
		   [dbo].[LOC_Order].[ShippingAddress],
		   [dbo].[LOC_Order].[UserID]
	FROM [dbo].[LOC_Order] 
	WHERE [dbo].[LOC_Order].[OrderID] = @OrderID;
END

exec [dbo].[PR_Order_SelectByPK] 3 


-- insert
ALTER PROCEDURE [dbo].[PR_Order_Insert]
    @OrderNumber INT,
    @OrderDate DATETIME,
    @CustomerID INT,
    @PaymentMode VARCHAR(100),
    @TotalAmount DECIMAL(10,2),
    @ShippingAddress VARCHAR(100),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[LOC_Order] 
	(
	[OrderNumber],
        [OrderDate],
        [CustomerID],
        [PaymentMode],
        [TotalAmount],
        [ShippingAddress],
        [UserID]
    )
    VALUES 
	(
	@OrderNumber,
        @OrderDate,
        @CustomerID,
        @PaymentMode,
        @TotalAmount,
        @ShippingAddress,
        @UserID
    );
END

EXEC [dbo].[PR_Order_Insert] 4,'08-09-2024 09:50 PM' , 2 ,'Credit Card' , 250.00 , 'line 3' , 3
EXEC [dbo].[PR_Order_SelectAll] 


-- update
CREATE PROCEDURE [dbo].[PR_Order_UpdateByPK]
    @OrderID int,
    @OrderDate DATETIME,
    @PaymentMode VARCHAR(100),
    @TotalAmount DECIMAL(10,2),
    @ShippingAddress VARCHAR(100)
AS
BEGIN
    UPDATE [dbo].[LOC_Order]
    SET [OrderDate] = @OrderDate,
	[PaymentMode] = @PaymentMode,
        [TotalAmount] = @TotalAmount,
        [ShippingAddress] = @ShippingAddress
    WHERE [dbo].[LOC_Order].[OrderID] = @OrderID;
END

EXEC [dbo].[PR_Order_UpdateByPK] 4,'08-09-2024 10:22PM',1,'Pay Pal',200.00,'line 2',3
EXEC [dbo].[PR_Order_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_Order_DeleteByPK] 
	@OrderID int 
AS
BEGIN
	DELETE 
	FROM [dbo].[LOC_Order] 
	WHERE [dbo].[LOC_Order].[OrderID] = @OrderID;
END

EXEC [dbo].[PR_Order_DeleteByPK] 4
EXEC [dbo].[PR_Order_SelectAll] 


-- dropdown
CREATE PROCEDURE [dbo].[PR_Order_DropDown]
AS
BEGIN
	SELECT 
		OrderID , 
		OrderNumber 
	FROM [dbo].[LOC_Order];
END
