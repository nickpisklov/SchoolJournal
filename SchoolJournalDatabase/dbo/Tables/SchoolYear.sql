CREATE TABLE [dbo].[SchoolYear] (
    [Id]        INT  IDENTITY (1, 1) NOT NULL,
    [StartDate] DATE NOT NULL,
    [EndDate]   DATE NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

