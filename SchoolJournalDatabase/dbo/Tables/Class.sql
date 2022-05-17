CREATE TABLE [dbo].[Class] (
    [Id]              INT          IDENTITY (1, 1) NOT NULL,
    [Title]           VARCHAR (10) NOT NULL,
    [RecruitmentDate] DATE         NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

