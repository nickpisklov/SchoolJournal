CREATE TABLE [dbo].[LessonTime] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [StartTime] CHAR (5) NOT NULL,
    [EndTime]   CHAR (5) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

