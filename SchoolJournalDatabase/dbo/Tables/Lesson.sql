CREATE TABLE [dbo].[Lesson] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [FK_Teacher]    INT           NOT NULL,
    [FK_Subject]    INT           NOT NULL,
    [FK_Class]      INT           NOT NULL,
    [FK_SchoolYear] INT           NOT NULL,
    [FK_LessonTime] INT           NOT NULL,
    [LessonDate]    DATE          NOT NULL,
    [Theme]         VARCHAR (250) NOT NULL,
    [Homework]      TEXT          DEFAULT (' ') NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LessonTimeLesson] FOREIGN KEY ([FK_LessonTime]) REFERENCES [dbo].[LessonTime] ([Id]),
    CONSTRAINT [FK_SubjectForTeacherLesson] FOREIGN KEY ([FK_Teacher], [FK_Subject], [FK_Class], [FK_SchoolYear]) REFERENCES [dbo].[SubjectForTeacher] ([FK_Teacher], [FK_Subject], [FK_Class], [FK_SchoolYear])
);

