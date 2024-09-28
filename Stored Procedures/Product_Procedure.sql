-- select all
alter procedure  [dbo].[PR_Product_SelectAll]
as
begin
	select 
		   [dbo].[LOC_Product].[ProductID] ,
		   [dbo].[LOC_Product].[ProductName] ,
		   [dbo].[LOC_Product].[ProductPrice] ,
		   [dbo].[LOC_Product].[ProductCode] ,
		   [dbo].[LOC_Product].[Description] ,
		   [dbo].[LOC_Product].[UserID] 
	from [dbo].[LOC_Product]

	inner join [dbo].[LOC_User] 
	on [dbo].[LOC_Product].[UserID] = [dbo].[LOC_User].[UserID]

	order by [dbo].[LOC_Product].[ProductName] ,
			 [dbo].[LOC_Product].[ProductPrice] ,
		     [dbo].[LOC_Product].[ProductCode] ,
		     [dbo].[LOC_Product].[Description]
end

exec [dbo].[PR_Product_SelectAll] 


-- by primary key
create procedure [dbo].[PR_Product_SelectByPK]
	@ProductID int
as
begin
	select 
		   [dbo].[LOC_Product].[ProductName] ,
		   [dbo].[LOC_Product].[ProductPrice] ,
		   [dbo].[LOC_Product].[ProductCode] ,
		   [dbo].[LOC_Product].[Description] ,
		   [dbo].[LOC_Product].[UserID]
	from [dbo].[LOC_Product] 
	where [dbo].[LOC_Product].[ProductID] = @ProductID ;
end

exec [dbo].[PR_Product_SelectByPK] 3 


-- insert
create procedure [dbo].[PR_Product_Insert]
	@ProductName varchar(100),
	@ProductPrice decimal(10,2),
	@ProductCode varchar(100),
	@Description varchar(100),
	@UserID int
as
begin
	insert into [dbo].[LOC_Product] 
	(
		[ProductName],
		[ProductPrice],
		[ProductCode],
		[Description],
		[UserID] 
	)
	values 
	(
		@ProductName,
		@ProductPrice,
		@ProductCode,
		@Description,
		@UserID 
	)
end

exec [dbo].[PR_Product_Insert] 'Product C', 22.00, 'C003', 'Description for Product C', 3
exec [dbo].[PR_Product_SelectAll] 


-- update
ALTER PROCEDURE [dbo].[PR_Product_UpdateByPK]
    @ProductID INT,
    @ProductName VARCHAR(100),
    @ProductPrice DECIMAL(10,2),
    @ProductCode VARCHAR(100),
    @Description VARCHAR(100)
AS
BEGIN
    UPDATE [dbo].[LOC_Product]
    SET [ProductName] = @ProductName,
        [ProductPrice] = @ProductPrice,
        [ProductCode] = @ProductCode,
        [Description] = @Description
    WHERE [dbo].[LOC_Product].[ProductID] = @ProductID;
END;

exec [dbo].[PR_Product_UpdateByPK] 4 , 'Product D' , 19.00 , D005 , 'Description D' , 
exec [dbo].[PR_Product_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_Product_DeleteByPK] 
	@ProductID int 
as
begin
	DELETE 
	FROM [dbo].[LOC_Product] 
	WHERE [dbo].[LOC_Product].[ProductID] = @ProductID
end

exec [dbo].[PR_Product_DeleteByPK] 4
exec [dbo].[PR_Product_SelectAll] 

-- dropdown
create procedure [dbo].[PR_Product_DropDown]
as
begin
	select 
		ProductID , 
		ProductName 
	from [dbo].[LOC_Product]
end