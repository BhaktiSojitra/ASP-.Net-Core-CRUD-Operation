-- select all
create procedure [dbo].[PR_User_SelectAll]
as
begin
	select 
		   [dbo].[LOC_User].[UserID] ,
		   [dbo].[LOC_User].[UserName] ,
		   [dbo].[LOC_User].[Email] ,
		   [dbo].[LOC_User].[Password] ,
		   [dbo].[LOC_User].[MobileNo] ,
		   [dbo].[LOC_User].[Address] ,
		   [dbo].[LOC_User].[IsActive]
	from [dbo].[LOC_User]
end

exec [dbo].[PR_User_SelectAll] 


-- by primary key
create procedure [dbo].[PR_User_SelectByPK]
	@UserID int
as
begin
	select 
	       [dbo].[LOC_User].[UserName] ,
		   [dbo].[LOC_User].[Email] ,
		   [dbo].[LOC_User].[Password] ,
		   [dbo].[LOC_User].[MobileNo] ,
		   [dbo].[LOC_User].[Address] ,
		   [dbo].[LOC_User].[IsActive]
	from [dbo].[LOC_User] 
	where [dbo].[LOC_User].[UserID] = @UserID ;
end

exec [dbo].[PR_User_SelectByPK] 2 


-- insert
create procedure [dbo].[PR_User_Insert]
	@UserName varchar(100),
	@Email varchar(100),
	@Password varchar(100),
	@MobileNo varchar(15),
	@Address varchar(100),
	@IsActive bit
as
begin
	insert into [dbo].[LOC_User] 
	(
		[UserName] ,
		[Email] ,
		[Password] ,
		[MobileNo] ,
		[Address] ,
		[IsActive] 
	)
	values 
	(
		@UserName ,
		@Email ,
		@Password ,
		@MobileNo ,
		@Address ,
		@IsActive 
	)
end

exec [dbo].[PR_User_Insert] 'lata', 'lata@gmail.com' ,'password992', '9925142078', 'line5', 1
exec [dbo].[PR_User_SelectAll] 


-- update
CREATE PROCEDURE [dbo].[PR_User_UpdateByPK]
	@UserID int,
    @UserName varchar(100),
	@Email varchar(100),
	@Password varchar(100),
	@MobileNo varchar(15),
	@Address varchar(100),
	@IsActive bit
AS
BEGIN
    UPDATE [dbo].[LOC_User]
    SET [UserName] = @UserName,
		[Email] = @Email,
		[Password] = @Password,
		[MobileNo] = @MobileNo,
		[Address] = @Address,
		[IsActive] = @IsActive
    WHERE [dbo].[LOC_User].[UserID] = @UserID;
END;

exec [dbo].[PR_User_UpdateByPK] 4 , 'bhakti' , 'sojitrabhakti03@gmail.com' , password902 , '9925142078' , 'line2' , 0
exec [dbo].[PR_User_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_User_DeleteByPK] 
	@UserID int 
as
begin
	DELETE 
	FROM [dbo].[LOC_User] 
	WHERE [dbo].[LOC_User].[UserID] = @UserID
end

exec [dbo].[PR_User_DeleteByPK] 4
exec [dbo].[PR_User_SelectAll] 


-- dropdown
create procedure [dbo].[PR_User_DropDown]
as
begin
	select 
		UserID , 
		UserName 
	from [dbo].[LOC_User]
end

--login
create procedure [dbo].[PR_User_Login]
    @UserName varchar(50),
    @Password varchar(50)
as
begin
    select 
        [dbo].[LOC_User].[UserID], 
        [dbo].[LOC_User].[UserName], 
        [dbo].[LOC_User].[MobileNo], 
        [dbo].[LOC_User].[Email], 
        [dbo].[LOC_User].[Password],
        [dbo].[LOC_User].[Address]
    from 
        [dbo].[LOC_User] 
    where 
        [dbo].[LOC_User].[UserName] = @UserName 
        AND [dbo].[LOC_User].[Password] = @Password;
end

--register
alter procedure [dbo].[PR_User_Register]
    @UserName varchar(50),
    @Password varchar(50),
    @Email varchar(500),
    @MobileNo varchar(50),
    @Address varchar(50),
	@IsActive bit = 1 -- Default value for IsActive (1 = true, 0 = false)
as
begin
    insert into [dbo].[LOC_User]
    (
        [UserName],
        [Password],
        [Email],
        [MobileNo],
        [Address],
		[IsActive]
    )
    values
    (
        @UserName,
        @Password,
        @Email,
        @MobileNo,
        @Address,
		@IsActive
    );
end