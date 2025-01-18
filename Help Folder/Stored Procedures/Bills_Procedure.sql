-- select all
CREATE PROCEDURE  [dbo].[PR_Bills_SelectAll]
AS
BEGIN
	SELECT 
		   [dbo].[LOC_Bills].[BillID],
		   [dbo].[LOC_Bills].[BillNumber],
		   [dbo].[LOC_Bills].[BillDate],
		   [dbo].[LOC_Bills].[OrderID],
		   [dbo].[LOC_Bills].[TotalAmount],
		   [dbo].[LOC_Bills].[Discount],
		   [dbo].[LOC_Bills].[NetAmount],
		   [dbo].[LOC_Bills].[UserID]
	FROM [dbo].[LOC_Bills]
	INNER JOIN [dbo].[LOC_Order] 
	ON [dbo].[LOC_Bills].[OrderID] = [dbo].[LOC_Order].[OrderID]
	INNER JOIN [dbo].[LOC_User] 
	ON [dbo].[LOC_Bills].[UserID] = [dbo].[LOC_User].[UserID]
	ORDER BY [dbo].[LOC_Bills].[BillNumber],
		     [dbo].[LOC_Bills].[BillDate],
			 [dbo].[LOC_Bills].[TotalAmount],
		     [dbo].[LOC_Bills].[Discount],
		     [dbo].[LOC_Bills].[NetAmount];
END

EXEC [dbo].[PR_Bills_SelectAll] 


-- by primary key
ALTER PROCEDURE [dbo].[PR_Bills_SelectByPK]
	@BillID int
AS
BEGIN
	SELECT 
	       [dbo].[LOC_Bills].[BillID],
	       [dbo].[LOC_Bills].[BillNumber],
		   [dbo].[LOC_Bills].[BillDate],
		   [dbo].[LOC_Bills].[OrderID],
		   [dbo].[LOC_Bills].[TotalAmount],
		   [dbo].[LOC_Bills].[Discount],
		   [dbo].[LOC_Bills].[NetAmount],
		   [dbo].[LOC_Bills].[UserID]
	FROM [dbo].[LOC_Bills] 
	WHERE [dbo].[LOC_Bills].[BillID] = @BillID;
END

EXEC [dbo].[PR_Bills_SelectByPK] 3 


-- insert
ALTER PROCEDURE [dbo].[PR_Bills_Insert]
	@BillNumber Varchar(100),
	@OrderID INT,
	@TotalAmount DECIMAL(10,2),
	@Discount decimal(10,2) ,
	@NetAmount decimal(10,2) ,
    @UserID INT
AS
BEGIN
    INSERT INTO [dbo].[LOC_Bills] 
	(
		[BillNumber] ,
		[BillDate] ,
		[OrderID] ,
		[TotalAmount] ,
		[Discount] ,
		[NetAmount] ,
		[UserID]
    )
    VALUES 
	(
		@BillNumber ,
		getdate() ,
		@OrderID ,
		@TotalAmount ,
		@Discount ,
		@NetAmount ,
		@UserID 
    );
END

EXEC [dbo].[PR_Bills_Insert] 'B123789',3,800.00,20.00,600.00,2
EXEC [dbo].[PR_Bills_SelectAll] 


-- update
ALTER PROCEDURE [dbo].[PR_Bills_UpdateByPK]
	@BillID int,
	@BillNumber Varchar(100),
	@TotalAmount DECIMAL(10,2),
	@Discount decimal(10,2),
	@NetAmount decimal(10,2)
AS
BEGIN
    UPDATE [dbo].[LOC_Bills]
    SET [BillNumber] = @BillNumber,
		[BillDate] = getdate(),
        [TotalAmount] = @TotalAmount,
		[Discount] = @Discount,
		[NetAmount] = @NetAmount
    WHERE [dbo].[LOC_Bills].[BillID] = @BillID;
END

EXEC [dbo].[PR_Bills_UpdateByPK] 4,'B34789',200.00,10.00,100.00
EXEC [dbo].[PR_Bills_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_Bills_DeleteByPK] 
	@BillID int
AS
BEGIN
	DELETE 
	FROM [dbo].[LOC_Bills] 
	WHERE [dbo].[LOC_Bills].[BillID] = @BillID;
END

EXEC [dbo].[PR_Bills_DeleteByPK] 4
EXEC [dbo].[PR_Bills_SelectAll] 
