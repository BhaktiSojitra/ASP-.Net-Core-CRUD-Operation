-- select all
CREATE PROCEDURE  [dbo].[PR_Customer_SelectAll]
AS
BEGIN
	SELECT 
		   [dbo].[LOC_Customer].[CustomerID],
		   [dbo].[LOC_Customer].[CustomerName],
		   [dbo].[LOC_Customer].[HomeAddress],
		   [dbo].[LOC_Customer].[Email],
		   [dbo].[LOC_Customer].[MobileNo],
		   [dbo].[LOC_Customer].[GSTNo],
		   [dbo].[LOC_Customer].[CityName],
		   [dbo].[LOC_Customer].[PinCode],
		   [dbo].[LOC_Customer].[NetAmount],
		   [dbo].[LOC_Customer].[UserID]
	FROM [dbo].[LOC_Customer]
	INNER JOIN [dbo].[LOC_User] 
	ON [dbo].[LOC_Customer].[UserID] = [dbo].[LOC_User].[UserID]
	ORDER BY [dbo].[LOC_Customer].[CustomerName],
		     [dbo].[LOC_Customer].[HomeAddress],
		     [dbo].[LOC_Customer].[Email],
		     [dbo].[LOC_Customer].[MobileNo],
		     [dbo].[LOC_Customer].[GSTNo],
		     [dbo].[LOC_Customer].[CityName],
		     [dbo].[LOC_Customer].[PinCode],
		     [dbo].[LOC_Customer].[NetAmount]; 
END

EXEC [dbo].[PR_Customer_SelectAll] 


-- by primary key
ALTER PROCEDURE [dbo].[PR_Customer_SelectByPK]
	@CustomerID int
AS
BEGIN
	SELECT 
	       [dbo].[LOC_Customer].[CustomerID],
		   [dbo].[LOC_Customer].[CustomerName],
		   [dbo].[LOC_Customer].[HomeAddress],
		   [dbo].[LOC_Customer].[Email],
		   [dbo].[LOC_Customer].[MobileNo],
		   [dbo].[LOC_Customer].[GSTNo],
		   [dbo].[LOC_Customer].[CityName],
		   [dbo].[LOC_Customer].[PinCode],
		   [dbo].[LOC_Customer].[NetAmount],
		   [dbo].[LOC_Customer].[UserID]
	FROM [dbo].[LOC_Customer] 
	WHERE [dbo].[LOC_Customer].[CustomerID] = @CustomerID;
END

EXEC [dbo].[PR_Customer_SelectByPK] 1 


-- insert
CREATE PROCEDURE [dbo].[PR_Customer_Insert]
	@CustomerName varchar(100),
	@HomeAddress varchar(100),
	@Email varchar(100),
	@MobileNo varchar(15),
	@GSTNo varchar(15),
	@CityName varchar(100),
	@PinCode varchar(15),
	@NetAmount decimal(10,2),
	@UserID int
AS
BEGIN
	INSERT INTO [dbo].[LOC_Customer] 
	(
		[CustomerName],
		[HomeAddress],
		[Email],
		[MobileNo],
		[GSTNo],
		[CityName],
		[PinCode],
		[NetAmount],
		[UserID] 
	)
	VALUES 
	(
		@CustomerName,
		@HomeAddress,
		@Email,
		@MobileNo,
		@GSTNo,
		@CityName,
		@PinCode,
		@NetAmount,
		@UserID 
	);
END

EXEC [dbo].[PR_Customer_Insert] 'xyz','line1','xyz@gmail.com','0123456789','GST577457A','USA','789023',2000.00,1
EXEC [dbo].[PR_Customer_SelectAll] 


-- update
CREATE PROCEDURE [dbo].[PR_Customer_UpdateByPK]
	@CustomerID int ,
	@CustomerName varchar(100),
	@HomeAddress varchar(100),
	@Email varchar(100),
	@MobileNo varchar(15),
	@GSTNo varchar(15),
	@CityName varchar(100),
	@PinCode varchar(15),
	@NetAmount decimal(10,2)
AS
BEGIN
    UPDATE [dbo].[LOC_Customer]
    SET [CustomerName]  = @CustomerName,
		[HomeAddress] = @HomeAddress,
		[Email] = @Email,
		[MobileNo] = @MobileNo,
		[GSTNo] = @GSTNo,
		[CityName] = @CityName,
		[PinCode] = @PinCode,
		[NetAmount] = @NetAmount
    WHERE [dbo].[LOC_Customer].[CustomerID] = @CustomerID;
END

EXEC [dbo].[PR_Customer_UpdateByPK] 4,'bhakti','line2','sojitrabhakti03@gmail.com','9925142078','GST88208','USA','36200',2000.50
EXEC [dbo].[PR_Customer_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_Customer_DeleteByPK] 
	@CustomerID int 
AS
BEGIN
	DELETE 
	FROM [dbo].[LOC_Customer] 
	WHERE [dbo].[LOC_Customer].[CustomerID] = @CustomerID;
END

EXEC [dbo].[PR_Customer_DeleteByPK]  7
EXEC [dbo].[PR_Customer_SelectAll] 

-- dropdown
CREATE PROCEDURE [dbo].[PR_Customer_DropDown]
AS
BEGIN
	SELECT 
		CustomerID , 
		CustomerName 
	FROM [dbo].[LOC_Customer];
END