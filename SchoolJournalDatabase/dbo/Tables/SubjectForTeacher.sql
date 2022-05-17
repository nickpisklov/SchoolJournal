CREATE TABLE [dbo].[SubjectForTeacher] (
    [FK_Teacher]    INT NOT NULL,
    [FK_Subject]    INT NOT NULL,
    [FK_Class]      INT NOT NULL,
    [FK_SchoolYear] INT NOT NULL,
    CONSTRAINT [PK_SubjectForTeacher] PRIMARY KEY CLUSTERED ([FK_Teacher] ASC, [FK_Subject] ASC, [FK_Class] ASC, [FK_SchoolYear] ASC),
    CONSTRAINT [FK_ClassSubjectForTeacher] FOREIGN KEY ([FK_Class]) REFERENCES [dbo].[Class] ([Id]),
    CONSTRAINT [FK_SchoolYearSubjectForTeacher] FOREIGN KEY ([FK_SchoolYear]) REFERENCES [dbo].[SchoolYear] ([Id]),
    CONSTRAINT [FK_SubjectSubjectForTeacher] FOREIGN KEY ([FK_Subject]) REFERENCES [dbo].[Subject] ([Id]),
    CONSTRAINT [FK_TeacherSubjectForTeacher] FOREIGN KEY ([FK_Teacher]) REFERENCES [dbo].[Teacher] ([Id])
);

