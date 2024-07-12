CREATE DATABASE user_service_db;

-- User Management Service Database

CREATE TABLE Schools (
    ID SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Address TEXT NOT NULL
);

CREATE TABLE Users (
    ID SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Role VARCHAR(50) NOT NULL CHECK (Role IN ('Student', 'Teacher', 'Director', 'Parent')),
    SchoolID INTEGER NOT NULL,
    FOREIGN KEY (SchoolID) REFERENCES School(ID)
);

CREATE TABLE ParentStudents (
    ID SERIAL PRIMARY KEY,
    ParentID INTEGER NOT NULL,
    StudentID INTEGER NOT NULL
);
