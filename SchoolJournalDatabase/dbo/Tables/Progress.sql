CREATE TABLE [dbo].[Progress] (
    [FK_Student] INT NOT NULL,
    [FK_Lesson]  INT NOT NULL,
    [FK_Mark]    INT NOT NULL,
    CONSTRAINT [PK_Progress] PRIMARY KEY CLUSTERED ([FK_Student] ASC, [FK_Lesson] ASC, [FK_Mark] ASC),
    CONSTRAINT [FK_LessonProgress] FOREIGN KEY ([FK_Lesson]) REFERENCES [dbo].[Lesson] ([Id]),
    CONSTRAINT [FK_MarkProgress] FOREIGN KEY ([FK_Mark]) REFERENCES [dbo].[Mark] ([Id]),
    CONSTRAINT [FK_StudentProgress] FOREIGN KEY ([FK_Student]) REFERENCES [dbo].[Student] ([Id])
);

