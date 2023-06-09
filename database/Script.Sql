CREATE DATABASE LoyMark;

CREATE TABLE Users
(
    Id INT NOT NULL PRIMARY KEY IDENTITY,
    Name NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(50) NOT NULL,
    BirthDate DATE NOT NULL,
    Phone BIGINT, 
    Country NVARCHAR(60) NOT NULL,
    ReceiveInformation BIT NOT NULL,
    Enabled BIT NOT NULL
);


CREATE TABLE Activities
(
    Id int NOT NULL PRIMARY KEY IDENTITY,
    CreateDate DATE NOT NULL,
    Task varchar(MAX) NOT NULL,
    Id_User int NOT NULL,
    CONSTRAINT FK_Activity_User FOREIGN KEY (Id_User) REFERENCES Users(Id)
);
