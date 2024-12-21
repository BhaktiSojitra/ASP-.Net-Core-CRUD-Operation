-- select all
CREATE PROCEDURE  [dbo].[PR_OrderDetail_SelectAll]
AS
BEGIN
	SELECT 
		   [dbo].[LOC_OrderDetail].[OrderDetailID],
		   [dbo].[LOC_OrderDetail].[OrderID],
		   [dbo].[LOC_OrderDetail].[ProductID],
		   [dbo].[LOC_OrderDetail].[Quantity],
		   [dbo].[LOC_OrderDetail].[Amount],
		   [dbo].[LOC_OrderDetail].[TotalAmount],
		   [dbo].[LOC_OrderDetail].[UserID]
	FROM [dbo].[LOC_OrderDetail]
	INNER JOIN [dbo].[LOC_Order] 
	ON [dbo].[LOC_OrderDetail].[OrderID] = [dbo].[LOC_Order].[OrderID]
	INNER JOIN [dbo].[LOC_Product] 
	ON [dbo].[LOC_OrderDetail].[ProductID] = [dbo].[LOC_Product].[ProductID]
	INNER JOIN [dbo].[LOC_User] 
	ON [dbo].[LOC_Order].[UserID] = [dbo].[LOC_User].[UserID]
	ORDER BY [dbo].[LOC_OrderDetail].[Quantity],
		     [dbo].[LOC_OrderDetail].[Amount],
		     [dbo].[LOC_OrderDetail].[TotalAmount]; 
END

EXEC [dbo].[PR_OrderDetail_SelectAll]  


-- by primary key
ALTER PROCEDURE [dbo].[PR_OrderDetail_SelectByPK]
	@OrderDetailID int
AS
BEGIN
	SELECT 
	       [dbo].[LOC_OrderDetail].[OrderDetailID],
		   [dbo].[LOC_OrderDetail].[OrderID],
		   [dbo].[LOC_OrderDetail].[ProductID],
		   [dbo].[LOC_OrderDetail].[Quantity],
		   [dbo].[LOC_OrderDetail].[Amount],
		   [dbo].[LOC_OrderDetail].[TotalAmount],
		   [dbo].[LOC_OrderDetail].[UserID]
	FROM [dbo].[LOC_OrderDetail] 
	WHERE [dbo].[LOC_OrderDetail].[OrderDetailID] = @OrderDetailID;
END

EXEC [dbo].[PR_OrderDetail_SelectByPK] 4  


-- insert
CREATE PROCEDURE [dbo].[PR_OrderDetail_Insert]
    @OrderID INT,
	@ProductID INT,
	@Quantity INT,
    @Amount DECIMAL(10,2),
	@TotalAmount DECIMAL(10,2),
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[LOC_OrderDetail] 
	(
        [OrderID],
        [ProductID],
        [Quantity],
        [Amount],
        [TotalAmount],
        [UserID]
    )
    VALUES 
	(
        @OrderID ,
		@ProductID ,
		@Quantity ,
		@Amount ,
		@TotalAmount ,
		@UserID
    );
END

EXEC [dbo].[PR_OrderDetail_Insert] 3,3,10,60.00,70.00,3
EXEC [dbo].[PR_OrderDetail_SelectAll] 


-- update
CREATE PROCEDURE [dbo].[PR_OrderDetail_UpdateByPK]
	@OrderDetailID int ,
	@Quantity INT,
    @Amount DECIMAL(10,2),
	@TotalAmount DECIMAL(10,2)
AS
BEGIN
    UPDATE [dbo].[LOC_OrderDetail]
    SET [Quantity] = @Quantity,
        [Amount] = @Amount,
        [TotalAmount] = @TotalAmount
    WHERE [dbo].[LOC_OrderDetail].[OrderDetailID] = @OrderDetailID;
END

EXEC [dbo].[PR_OrderDetail_UpdateByPK] 5,12,59.00,60.00
EXEC [dbo].[PR_OrderDetail_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_OrderDetail_DeleteByPK] 
	@OrderDetailID int 
AS
BEGIN
	DELETE 
	FROM [dbo].[LOC_OrderDetail] 
	WHERE [dbo].[LOC_OrderDetail].[OrderDetailID] = @OrderDetailID;
END

EXEC [dbo].[PR_OrderDetail_DeleteByPK] 5
EXEC [dbo].[PR_OrderDetail_SelectAll] 