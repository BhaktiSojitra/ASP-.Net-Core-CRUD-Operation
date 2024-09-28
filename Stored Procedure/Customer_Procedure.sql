-- select all
create procedure  [dbo].[PR_Customer_SelectAll]
as
begin
	select 
		   [dbo].[LOC_Customer].[CustomerID] ,
		   [dbo].[LOC_Customer].[CustomerName] ,
		   [dbo].[LOC_Customer].[HomeAddress] ,
		   [dbo].[LOC_Customer].[Email] ,
		   [dbo].[LOC_Customer].[MobileNo] ,
		   [dbo].[LOC_Customer].[GSTNo] ,
		   [dbo].[LOC_Customer].[CityName] ,
		   [dbo].[LOC_Customer].[PinCode] ,
		   [dbo].[LOC_Customer].[NetAmount] ,
		   [dbo].[LOC_Customer].[UserID]
	from [dbo].[LOC_Customer]

	inner join [dbo].[LOC_User] 
	on [dbo].[LOC_Customer].[UserID] = [dbo].[LOC_User].[UserID]

	order by [dbo].[LOC_Customer].[CustomerName] ,
		     [dbo].[LOC_Customer].[HomeAddress] ,
		     [dbo].[LOC_Customer].[Email] ,
		     [dbo].[LOC_Customer].[MobileNo] ,
		     [dbo].[LOC_Customer].[GSTNo] ,
		     [dbo].[LOC_Customer].[CityName] ,
		     [dbo].[LOC_Customer].[PinCode] ,
		     [dbo].[LOC_Customer].[NetAmount] 
end

exec [dbo].[PR_Customer_SelectAll] 


-- by primary key
create procedure [dbo].[PR_Customer_SelectByPK]
	@CustomerID int
as
begin
	select 
		   [dbo].[LOC_Customer].[CustomerName] ,
		   [dbo].[LOC_Customer].[HomeAddress] ,
		   [dbo].[LOC_Customer].[Email] ,
		   [dbo].[LOC_Customer].[MobileNo] ,
		   [dbo].[LOC_Customer].[GSTNo] ,
		   [dbo].[LOC_Customer].[CityName] ,
		   [dbo].[LOC_Customer].[PinCode] ,
		   [dbo].[LOC_Customer].[NetAmount] ,
		   [dbo].[LOC_Customer].[UserID]
	from [dbo].[LOC_Customer] where [dbo].[LOC_Customer].[CustomerID] = @CustomerID ;
end

exec [dbo].[PR_Customer_SelectByPK] 1 


-- insert
create procedure [dbo].[PR_Customer_Insert]
	@CustomerName varchar(100),
	@HomeAddress varchar(100),
	@Email varchar(100),
	@MobileNo varchar(15),
	@GSTNo varchar(15),
	@CityName varchar(100),
	@PinCode varchar(15),
	@NetAmount decimal(10,2),
	@UserID int
as
begin
	insert into [dbo].[LOC_Customer] 
	(
		[CustomerName] ,
		[HomeAddress] ,
		[Email] ,
		[MobileNo] ,
		[GSTNo] ,
		[CityName] ,
		[PinCode] ,
		[NetAmount] ,
		[UserID] 
	)
	values 
	(
		@CustomerName ,
		@HomeAddress ,
		@Email ,
		@MobileNo ,
		@GSTNo ,
		@CityName ,
		@PinCode ,
		@NetAmount ,
		@UserID 
	)
end

exec [dbo].[PR_Customer_Insert] 'xyz','line1','xyz@gmail.com','0123456789','GST577457A','USA','789023',2000.00,6
exec [dbo].[PR_Customer_SelectAll] 


-- update
create procedure [dbo].[PR_Customer_UpdateByPK]
	@CustomerID int ,
	@CustomerName varchar(100),
	@HomeAddress varchar(100),
	@Email varchar(100),
	@MobileNo varchar(15),
	@GSTNo varchar(15),
	@CityName varchar(100),
	@PinCode varchar(15),
	@NetAmount decimal(10,2)
as
begin
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
END;

exec [dbo].[PR_Customer_UpdateByPK] 4,'bhakti','line2','sojitrabhakti03@gmail.com','9925142078','GST88208','USA','36200',2000.50
exec [dbo].[PR_Customer_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_Customer_DeleteByPK] 
	@CustomerID int 
as
begin
	DELETE 
	FROM [dbo].[LOC_Customer] 
	WHERE [dbo].[LOC_Customer].[CustomerID] = @CustomerID
end

exec [dbo].[PR_Customer_DeleteByPK]  7
exec [dbo].[PR_Customer_SelectAll] 

-- dropdown
create procedure [dbo].[PR_Customer_DropDown]
as
begin
	select 
		CustomerID , 
		CustomerName 
	from [dbo].[LOC_Customer]
end