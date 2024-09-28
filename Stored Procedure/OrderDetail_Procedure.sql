-- select all
create procedure  [dbo].[PR_OrderDetail_SelectAll]
as
begin
	select 
		   [dbo].[LOC_OrderDetail].[OrderDetailID] ,
		   [dbo].[LOC_OrderDetail].[OrderID] ,
		   [dbo].[LOC_OrderDetail].[ProductID] ,
		   [dbo].[LOC_OrderDetail].[Quantity] ,
		   [dbo].[LOC_OrderDetail].[Amount] ,
		   [dbo].[LOC_OrderDetail].[TotalAmount] ,
		   [dbo].[LOC_OrderDetail].[UserID]
	from [dbo].[LOC_OrderDetail]

	inner join [dbo].[LOC_Order] 
	on [dbo].[LOC_OrderDetail].[OrderID] = [dbo].[LOC_Order].[OrderID]

	inner join [dbo].[LOC_Product] 
	on [dbo].[LOC_OrderDetail].[ProductID] = [dbo].[LOC_Product].[ProductID]

	inner join [dbo].[LOC_User] 
	on [dbo].[LOC_Order].[UserID] = [dbo].[LOC_User].[UserID]

	order by [dbo].[LOC_OrderDetail].[Quantity] ,
		     [dbo].[LOC_OrderDetail].[Amount] ,
		     [dbo].[LOC_OrderDetail].[TotalAmount] 
end

exec [dbo].[PR_OrderDetail_SelectAll]  


-- by primary key
create procedure [dbo].[PR_OrderDetail_SelectByPK]
	@OrderDetailID int
as
begin
	select 
		   [dbo].[LOC_OrderDetail].[OrderID] ,
		   [dbo].[LOC_OrderDetail].[ProductID] ,
		   [dbo].[LOC_OrderDetail].[Quantity] ,
		   [dbo].[LOC_OrderDetail].[Amount] ,
		   [dbo].[LOC_OrderDetail].[TotalAmount] ,
		   [dbo].[LOC_OrderDetail].[UserID]
	from [dbo].[LOC_OrderDetail] where [dbo].[LOC_OrderDetail].[OrderDetailID] = @OrderDetailID ;
end

exec [dbo].[PR_OrderDetail_SelectByPK] 2  


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

exec [dbo].[PR_OrderDetail_Insert] 3,3,10,60.00,70.00,3
exec [dbo].[PR_OrderDetail_SelectAll] 


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

exec [dbo].[PR_OrderDetail_UpdateByPK] 5,12,59.00,60.00
exec [dbo].[PR_OrderDetail_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_OrderDetail_DeleteByPK] 
	@OrderDetailID int 
as
begin
	DELETE 
	FROM [dbo].[LOC_OrderDetail] 
	WHERE [dbo].[LOC_OrderDetail].[OrderDetailID] = @OrderDetailID
end

exec [dbo].[PR_OrderDetail_DeleteByPK] 5
exec [dbo].[PR_OrderDetail_SelectAll] 