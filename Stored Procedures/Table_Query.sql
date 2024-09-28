CREATE DATABASE CoffeeShopManagementSystem

CREATE TABLE LOC_Product (
    ProductID INT primary key identity(1,1) NOT NULL ,
    ProductName VARCHAR(100) NOT NULL,
    ProductPrice DECIMAL(10,2) NOT NULL,
    ProductCode VARCHAR(100) NOT NULL,
    Description VARCHAR(100) NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES LOC_User(UserID)
);

CREATE TABLE LOC_User (
    UserID INT  primary key IDENTITY(1,1) not null,
    UserName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    MobileNo VARCHAR(15) NOT NULL,
    Address VARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL,
);

CREATE TABLE LOC_Order (
    OrderID INT PRIMARY KEY Identity(1,1) NOT NULL,
	OrderNumber INT NOT NULL,
    OrderDate datetime NOT NULL,
    CustomerID INT NOT NULL,
    PaymentMode varchar(100) ,
    TotalAmount decimal(10,2) ,
    ShippingAddress VARCHAR(100) NOT NULL,
	UserID INT NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES LOC_Customer(CustomerID),
    FOREIGN KEY (UserID) REFERENCES LOC_User(UserID)
);

CREATE TABLE LOC_OrderDetail (
    OrderDetailID INT PRIMARY KEY Identity(1,1) NOT NULL,
    OrderID int NOT NULL,
    ProductID int NOT NULL,
    Quantity int NOT NULL,
    Amount decimal(10,2) NOT NULL,
	TotalAmount decimal(10,2) NOT NULL,
	UserID INT NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES LOC_Order(OrderID),
	FOREIGN KEY (ProductID) REFERENCES LOC_Product(ProductID),
    FOREIGN KEY (UserID) REFERENCES LOC_User(UserID)
);

CREATE TABLE LOC_Bills (
    BillID INT PRIMARY KEY Identity(1,1) NOT NULL,
    BillNumber varchar(100) NOT NULL,
    BillDate datetime NOT NULL,
    OrderID int not null ,
    TotalAmount decimal(10,2) not null,
	Discount decimal(10,2) ,
    NetAmount VARCHAR(100) NOT NULL,
	UserID INT NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES LOC_Order(OrderID),
    FOREIGN KEY (UserID) REFERENCES LOC_User(UserID)
);

CREATE TABLE LOC_Customer (
    CustomerID INT NOT NULL IDENTITY(1,1),
    CustomerName VARCHAR(100) NOT NULL,
    HomeAddress VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    MobileNo VARCHAR(15) NOT NULL,
    GSTNO VARCHAR(15) NOT NULL,
    CityName VARCHAR(100) NOT NULL,
    PinCode VARCHAR(15) NOT NULL,
    NetAmount DECIMAL(10,2) NOT NULL,
    UserID INT NOT NULL,
    PRIMARY KEY (CustomerID),
    FOREIGN KEY (UserID) REFERENCES LOC_User(UserID)
);
