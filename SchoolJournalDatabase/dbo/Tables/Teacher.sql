CREATE TABLE [dbo].[Teacher] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Login]      VARCHAR (100) NOT NULL,
    [Password]   VARCHAR (200) NOT NULL,
    [Name]       VARCHAR (100) NOT NULL,
    [Surname]    VARCHAR (100) NOT NULL,
    [Middlename] VARCHAR (100) NOT NULL,
    [HireDate]   DATE          NOT NULL,
    [FireDate]   DATE          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

