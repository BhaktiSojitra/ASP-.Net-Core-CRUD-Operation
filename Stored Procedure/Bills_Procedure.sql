-- select all
create procedure  [dbo].[PR_Bills_SelectAll]
as
begin
	select 
		   [dbo].[LOC_Bills].[BillID] ,
		   [dbo].[LOC_Bills].[BillNumber] ,
		   [dbo].[LOC_Bills].[BillDate] ,
		   [dbo].[LOC_Bills].[OrderID] ,
		   [dbo].[LOC_Bills].[TotalAmount] ,
		   [dbo].[LOC_Bills].[Discount] ,
		   [dbo].[LOC_Bills].[NetAmount] ,
		   [dbo].[LOC_Bills].[UserID]
	from [dbo].[LOC_Bills]

	inner join [dbo].[LOC_Order] 
	on [dbo].[LOC_Bills].[OrderID] = [dbo].[LOC_Order].[OrderID]

	inner join [dbo].[LOC_User] 
	on [dbo].[LOC_Bills].[UserID] = [dbo].[LOC_User].[UserID]

	order by [dbo].[LOC_Bills].[BillNumber] ,
		     [dbo].[LOC_Bills].[BillDate] ,
			 [dbo].[LOC_Bills].[TotalAmount] ,
		     [dbo].[LOC_Bills].[Discount] ,
		     [dbo].[LOC_Bills].[NetAmount]
end

exec [dbo].[PR_Bills_SelectAll] 


-- by primary key
create procedure [dbo].[PR_Bills_SelectByPK]
	@BillID int
as
begin
	select 
	       [dbo].[LOC_Bills].[BillNumber] ,
		   [dbo].[LOC_Bills].[BillDate] ,
		   [dbo].[LOC_Bills].[OrderID] ,
		   [dbo].[LOC_Bills].[TotalAmount] ,
		   [dbo].[LOC_Bills].[Discount] ,
		   [dbo].[LOC_Bills].[NetAmount] ,
		   [dbo].[LOC_Bills].[UserID]
	from [dbo].[LOC_Bills] where [dbo].[LOC_Bills].[BillID] = @BillID ;
end

exec [dbo].[PR_Bills_SelectByPK] 3 


-- insert
CREATE PROCEDURE [dbo].[PR_Bills_Insert]
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

exec [dbo].[PR_Bills_Insert] 'B45678',3,800.00,20.00,600.00,3
exec [dbo].[PR_Bills_SelectAll] 


-- update
ALTER PROCEDURE [dbo].[PR_Bills_UpdateByPK]
	@BillID int,
	@BillNumber Varchar(100),
	@BillDate DateTime,
	@TotalAmount DECIMAL(10,2),
	@Discount decimal(10,2),
	@NetAmount decimal(10,2)
AS
BEGIN
    UPDATE [dbo].[LOC_Bills]
    SET [BillNumber] = @BillNumber,
		[BillDate] = @BillDate,
        [TotalAmount] = @TotalAmount,
		[Discount] = @Discount,
		[NetAmount] = @NetAmount
    WHERE [dbo].[LOC_Bills].[BillID] = @BillID;
END

exec [dbo].[PR_Bills_UpdateByPK] 4,B34789,200.00,10.00,100.00
exec [dbo].[PR_Bills_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_Bills_DeleteByPK] 
	@BillID int
as
begin
	DELETE 
	FROM [dbo].[LOC_Bills] 
	WHERE [dbo].[LOC_Bills].[BillID] = @BillID
end

exec [dbo].[PR_Bills_DeleteByPK] 4
exec [dbo].[PR_Bills_SelectAll] 