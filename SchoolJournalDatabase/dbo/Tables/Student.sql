CREATE TABLE [dbo].[Student] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [FK_Class]   INT           NOT NULL,
    [Login]      VARCHAR (100) NOT NULL,
    [Password]   VARCHAR (200) NOT NULL,
    [Name]       VARCHAR (100) NOT NULL,
    [Surname]    VARCHAR (100) NOT NULL,
    [Middlename] VARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ClassStudent] FOREIGN KEY ([FK_Class]) REFERENCES [dbo].[Class] ([Id])
);

