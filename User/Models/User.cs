namespace User.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public int SchoolID { get; set; }

}

// CREATE TABLE Users (
//     ID SERIAL PRIMARY KEY,
//     Name VARCHAR(255) NOT NULL,
//     Email VARCHAR(255) NOT NULL,
//     Role VARCHAR(50) NOT NULL CHECK (Role IN ('Student', 'Teacher', 'Director', 'Parent')),
//     SchoolID INTEGER NOT NULL,
//     FOREIGN KEY (SchoolID) REFERENCES School(ID)
// );