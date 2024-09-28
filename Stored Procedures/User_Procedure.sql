-- select all
CREATE PROCEDURE [dbo].[PR_User_SelectAll]
AS
BEGIN
	SELECT 
		   [dbo].[LOC_User].[UserID],
		   [dbo].[LOC_User].[UserName],
		   [dbo].[LOC_User].[Email],
		   [dbo].[LOC_User].[Password],
		   [dbo].[LOC_User].[MobileNo],
		   [dbo].[LOC_User].[Address],
		   [dbo].[LOC_User].[IsActive]
	FROM [dbo].[LOC_User];
END

EXEC [dbo].[PR_User_SelectAll] 


-- by primary key
CREATE PROCEDURE [dbo].[PR_User_SelectByPK]
	@UserID int
AS
BEGIN
	SELECT 
	       [dbo].[LOC_User].[UserName],
		   [dbo].[LOC_User].[Email],
		   [dbo].[LOC_User].[Password],
		   [dbo].[LOC_User].[MobileNo],
		   [dbo].[LOC_User].[Address],
		   [dbo].[LOC_User].[IsActive]
	FROM [dbo].[LOC_User] 
	WHERE [dbo].[LOC_User].[UserID] = @UserID;
END

EXEC [dbo].[PR_User_SelectByPK] 2 


-- insert
CREATE PROCEDURE [dbo].[PR_User_Insert]
	@UserName varchar(100),
	@Email varchar(100),
	@Password varchar(100),
	@MobileNo varchar(15),
	@Address varchar(100),
	@IsActive bit
AS
BEGIN
	INSERT INTO [dbo].[LOC_User] 
	(
		[UserName],
		[Email],
		[Password],
		[MobileNo],
		[Address],
		[IsActive] 
	)
	VALUES 
	(
		@UserName,
		@Email,
		@Password,
		@MobileNo,
		@Address,
		@IsActive 
	);
END

EXEC [dbo].[PR_User_Insert] 'lata', 'lata@gmail.com' ,'password992', '9925142078', 'line5', 1
EXEC [dbo].[PR_User_SelectAll] 


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

EXEC [dbo].[PR_User_UpdateByPK] 4 , 'bhakti' , 'sojitrabhakti03@gmail.com' , password902 , '9925142078' , 'line2' , 0
EXEC [dbo].[PR_User_SelectAll] 


-- delete
CREATE PROCEDURE [dbo].[PR_User_DeleteByPK] 
	@UserID int 
AS
BEGIN
	DELETE 
	FROM [dbo].[LOC_User] 
	WHERE [dbo].[LOC_User].[UserID] = @UserID;
END

EXEC [dbo].[PR_User_DeleteByPK] 4
EXEC [dbo].[PR_User_SelectAll] 


-- dropdown
CREATE PROCEDURE [dbo].[PR_User_DropDown]
AS
BEGIN
	SELECT 
		UserID , 
		UserName 
	FROM [dbo].[LOC_User];
END

--login
CREATE PROCEDURE [dbo].[PR_User_Login]
    @UserName varchar(50),
    @Password varchar(50)
AS
BEGIN
    SELECT 
        [dbo].[LOC_User].[UserID], 
        [dbo].[LOC_User].[UserName], 
        [dbo].[LOC_User].[MobileNo], 
        [dbo].[LOC_User].[Email], 
        [dbo].[LOC_User].[Password],
        [dbo].[LOC_User].[Address]
    FROM 
        [dbo].[LOC_User] 
    WHERE 
        [dbo].[LOC_User].[UserName] = @UserName 
        AND [dbo].[LOC_User].[Password] = @Password;
END

--register
CREATE PROCEDURE [dbo].[PR_User_Register]
    @UserName varchar(50),
    @Password varchar(50),
    @Email varchar(500),
    @MobileNo varchar(50),
    @Address varchar(50),
	@IsActive bit = 1 -- Default value for IsActive (1 = true, 0 = false)
AS
BEGIN
    INSERT INTO [dbo].[LOC_User]
    (
        [UserName],
        [Password],
        [Email],
        [MobileNo],
        [Address],
		[IsActive]
    )
    VALUES
    (
        @UserName,
        @Password,
        @Email,
        @MobileNo,
        @Address,
		@IsActive
    );
END