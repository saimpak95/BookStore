create database BookStoreDB


 create table Authors(
 
 Id int primary key identity(1,1),
 FirstName nvarchar(50),
 LastName nvarchar(50),
 Bio nvarchar(max)
 
 )

 create table Books(
 
 Id int primary key identity(1,1),
 Title nvarchar(100),
 Year int,
 ISBN nvarchar(50),
 Summary nvarchar(150),
 Image nvarchar(150),
 Price money,
 AuthorID int references Authors(Id) on delete set null,
 
 )
