# Nimap-Project
Crud Operation in DotNet
Here are the MySql commands which have been used in the project:
mysql> show databases;

mysql> create database crud2;

mysql> use crud2;

mysql> show tables;
Empty set (0.00 sec)

mysql> CREATE TABLE Category (
       CategoryId INT PRIMARY KEY AUTO_INCREMENT,
       CategoryName VARCHAR(255)
       );

mysql> CREATE TABLE Product (
       ProductId INT PRIMARY KEY AUTO_INCREMENT,
       ProductName VARCHAR(50),
       CategoryId INT,
       FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
       );

mysql> ALTER TABLE Product
       ADD CONSTRAINT FK_Product_Category
       FOREIGN KEY (CategoryId)
       REFERENCES Category(CategoryId);

mysql> ALTER TABLE Category
       ADD CONSTRAINT PK_Category PRIMARY KEY (CategoryId);
