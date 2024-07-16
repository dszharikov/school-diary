namespace Grade.Models;

public class TermAssessment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int AssessmentTypeId { get; set; }
    public AssessmentType? AssessmentType { get; set; }
    public string GradeValue { get; set; } = null!;
    public int TermId { get; set; }
}

/*
CREATE TABLE TermAssessment (
    TermAssessmentID SERIAL PRIMARY KEY,
    StudentID INTEGER NOT NULL,
    SubjectID INTEGER NOT NULL,
    AssessmentTypeID INTEGER NOT NULL,
    GradeValue VARCHAR(2) NOT NULL,
    TermID INTEGER NOT NULL,
    FOREIGN KEY (StudentID) REFERENCES "User"(UserID),
    FOREIGN KEY (SubjectID) REFERENCES Subject(SubjectID),
    FOREIGN KEY (AssessmentTypeID) REFERENCES AssessmentType(AssessmentTypeID),
    FOREIGN KEY (TermID) REFERENCES Term(TermID)
);

*/