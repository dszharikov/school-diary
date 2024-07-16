namespace Grade.Models;

public class Grade
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassSubjectId { get; set; }
    public string GradeValue { get; set; } = null!;
    public DateOnly Date { get; set; }
}

/*
-- Grading Service Database

CREATE TABLE Grade (
    GradeID SERIAL PRIMARY KEY,
    StudentID INTEGER NOT NULL,
    ClassSubjectID INTEGER NOT NULL,
    GradeValue VARCHAR(2) NOT NULL,
    Date DATE NOT NULL
);

CREATE TABLE QuarterlyGrade (
    QuarterlyGradeID SERIAL PRIMARY KEY,
    StudentID INTEGER NOT NULL,
    SubjectID INTEGER NOT NULL,
    GradeValue VARCHAR(2) NOT NULL,
    TermID INTEGER NOT NULL
);

*/