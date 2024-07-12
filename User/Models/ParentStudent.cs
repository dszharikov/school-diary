namespace User.Models;

public class ParentStudent
{
    public int Id { get; set; }
    public int ParentId { get; set; }
    public int StudentId { get; set; }
}

// CREATE TABLE ParentStudents (
//     ID SERIAL PRIMARY KEY,
//     ParentID INTEGER NOT NULL,
//     StudentID INTEGER NOT NULL
// );