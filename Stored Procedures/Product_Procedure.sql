-- select all
CREATE PROCEDURE [dbo].[PR_Product_SelectAll]
AS
BEGIN
	SELECT 
		   [dbo].[LOC_Product].[ProductID],
		   [dbo].[LOC_Product].[ProductName],
		   [dbo].[LOC_Product].[ProductPrice],
		   [dbo].[LOC_Product].[ProductCode],
		   [dbo].[LOC_Product].[Description],
		   [dbo].[LOC_Product].[UserID] 
	FROM [dbo].[LOC_Product]
	INNER JOIN [dbo].[LOC_User] 
	ON [dbo].[LOC_Product].[UserID] = [dbo].[LOC_User].[UserID]
	ORDER BY [dbo].[LOC_Product].[ProductName],
			 [dbo].[LOC_Product].[ProductPrice],
		     [dbo].[LOC_Product].[ProductCode],
		     [dbo].[LOC_Product].[Description];
END

EXEC [dbo].[PR_Product_SelectAll] 


-- by primary key
CREATE PROCEDURE [dbo].[PR_Product_SelectByPK]
	@ProductID int
AS
BEGIN
	SELECT 
		   [dbo].[LOC_Product].[ProductName],
		   [dbo].[LOC_Product].[ProductPrice],
		   [dbo].[LOC_Product].[ProductCode],
		   [dbo].[LOC_Product].[Description],
		   [dbo].[LOC_Product].[UserID]
	FROM [dbo].[LOC_Product] 
	WHERE [dbo].[LOC_Product].[ProductID] = @ProductID;
END

EXEC [dbo].[PR_Product_SelectByPK] 3 


-- insert
CREATE PROCEDURE [dbo].[PR_Product_Insert]
	@ProductName varchar(100),
	@ProductPrice decimal(10,2),
	@ProductCode varchar(100),
	@Description varchar(100),
	@UserID int
AS
BEGIN
	INSERT INTO [dbo].[LOC_Product] 
	(
		[ProductName],
		[ProductPrice],
		[ProductCode],
		[Description],
		[UserID] 
	)
	VALUES 
	(
		@ProductName,
		@ProductPrice,
		@ProductCode,
		@Description,
		@UserID 
	);
END

EXEC [dbo].[PR_Product_Insert] 'Product C', 22.00, 'C003', 'Description for Product C', 3
EXEC [dbo].[PR_Product_SelectAll] 


-- update
CREATE PROCEDURE [dbo].[PR_Product_UpdateByPK]
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

EXEC [dbo].[PR_Product_UpdateByPK] 4 , 'Product D' , 19.00 , D005 , 'Description D' , 
EXEC [dbo].[PR_Product_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_Product_DeleteByPK] 
	@ProductID int 
AS
BEGIN
	DELETE 
	FROM [dbo].[LOC_Product] 
	WHERE [dbo].[LOC_Product].[ProductID] = @ProductID
END

EXEC [dbo].[PR_Product_DeleteByPK] 4
EXEC [dbo].[PR_Product_SelectAll] 

-- dropdown
CREATE PROCEDURE [dbo].[PR_Product_DropDown]
AS
BEGIN
	SELECT 
		ProductID , 
		ProductName 
	FROM [dbo].[LOC_Product];
END