﻿Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'DrivingSchoolDB')
BEGIN
DROP DATABASE DrivingSchoolDB;
END

Create Database DrivingSchoolDB
Go
Use DrivingSchoolDB
Go

Create Table Statuses 
(
StatusID int Primary Key Identity(1,1),
StatusDescription nvarchar(50) Not Null,
)

Create Table Manager
(
UserManagerID int Primary Key Identity(1,1),
SchoolName nvarchar(50) Not Null,
FirstName nvarchar(50) Not Null,
LastName nvarchar(50) Not Null,
ManagerEmail nvarchar(50) Unique Not Null,
ManagerPass nvarchar(50) Not Null,
ManagerStatus int Not Null Foreign Key References Statuses(StatusID),
ManagerID nvarchar(50) Not Null,
SchoolAddress nvarchar(50) Not Null,
ManagerPhone nvarchar(10) Not Null,
SchoolPhone nvarchar(10) Not Null,
ProfilePic nvarchar(50) Null,

)

Create table Package
(
PackageID int Primary Key Identity(1,1),
ManagerID int Not Null Foreign Key References Manager(UserManagerID),
Title nvarchar(100) Not Null,
TheText nvarchar(300) Not Null,
)

Create table Teacher 
(
UserTeacherID int Primary Key Identity(1,1),
SchoolName nvarchar(50) Not Null,  
TeacherEmail nvarchar(50) Unique Not Null,
--לשאול את עופר אם זה בסדר שזה אותו השם
FirstName nvarchar(50) Not Null,
LastName nvarchar(50) Not Null,
TeacherPass nvarchar(50) Not Null,
TeacherStatus int Not Null Foreign Key References Statuses(StatusID),
TeacherID nvarchar(50) Not Null,
WayToPay nvarchar(50) Not Null,
PhoneNumber nvarchar(50) Not Null,
Gender nvarchar(50) Not Null,
ProfilePic nvarchar(500) Null,
DrivingTechnic nvarchar(50) Not Null,
ManagerID int Not Null Foreign Key References Manager(UserManagerID),
)

Create table Student
(
UserStudentID int Primary Key Identity(1,1),
FirstName nvarchar(50) Not Null,
LastName nvarchar(50) Not Null,
SchoolName nvarchar(50) Not Null,  
StudentStatus int Not Null Foreign Key References Statuses(StatusID),
StudentEmail nvarchar(50) Unique Not Null,
StudentPass nvarchar(50) Not Null,
StudentLanguage nvarchar(50) Not Null,
DateOfTheory Date Not Null,
LengthOfLesson int Not Null,
TeacherID int Not Null Foreign Key References Teacher(UserTeacherID),
DrivingTechnic nvarchar(50) Not Null,
Gender nvarchar(50) Not Null,
StudentId nvarchar(50) Not Null,
DateOfBirth Date Not Null,
PhoneNumber nvarchar(10) Not Null,
CurrentLessonNum int Not Null,
InternalTestDone bit Not Null Default 1,
StudentAddress nvarchar(50) Not Null,
ProfilePic nvarchar(500) Null,
PackageID int Not Null Foreign Key References Package(PackageID),
)

Create table Comment
(
CommentID int Primary Key Identity(1,1),
StudentID int Not Null Foreign Key References Student(UserStudentID),
TeacherID int Not Null Foreign Key References Manager(UserManagerID),
TheText nvarchar(200) Not Null,
)

Create table TeacherSchedule
(
ScheduleID int Primary Key Identity(1,1),
DayOfSchedule nvarchar(15) Not Null,
Beginning nvarchar(15) Not Null,
LessonLength nvarchar(2) Not Null,
Ending nvarchar(15) Not Null,
TeacherID int Not Null Foreign Key References Teacher(UserTeacherId),
)

Create Table Tests
(
TestID int Primary Key Identity(1,1),
StudentID int Not Null Foreign Key References Student(UserStudentID),
ManagerID int Not Null Foreign Key References Manager(UserManagerID),
DateOfTest Datetime Not Null,
PassedOrNot bit Not Null Default 1,
comments nvarchar(500) Not Null,
)

Create Table Lesson 
(
LessonID int Primary Key Identity(1,1),
DateOfLesson DateTime Not Null,
StudentID int Not Null Foreign Key References Student(UserStudentId),
TeacherID int Not Null Foreign Key References Teacher(UserTeacherId),
PickUpLoc nvarchar(50) Not Null,
DropOffLoc nvarchar(50) Not Null,
DidExist bit Not Null Default 1,
)

Create Table HomePage 
(
HomePageText nvarchar(2000) Not Null,
UpdateTime Datetime Not Null,
)

INSERT INTO Statuses(StatusDescription)
VALUES('Pending')
SELECT * FROM Statuses
INSERT INTO Statuses(StatusDescription)
VALUES('Approved')
INSERT INTO Statuses(StatusDescription)
VALUES('Declined')

INSERT INTO Manager(FirstName, LastName, ManagerEmail, ManagerPass, ManagerStatus,ManagerID, SchoolAddress,  ManagerPhone, SchoolPhone, SchoolName)
VALUES('Itzik', 'Rotem', 'Rotem@gmail.com', '123', 2, '234567890', 'Golda Meir', '0537786549', '03456789', 'Ramon')
SELECT * FROM Manager

INSERT INTO Teacher(SchoolName, TeacherEmail, FirstName, LastName, TeacherPass,TeacherStatus, TeacherID,  WayToPay, PhoneNumber, Gender, ManagerID, DrivingTechnic)
VALUES('Ramon', 'Marom@gmail.com', 'Marom', 'Hai', 'm123',2, '111111111', 'Cash', '000000000', 'female', 1, 'Autumat')
SELECT * FROM Teacher

INSERT INTO Package(ManagerID, Title, TheText)
VALUES(1, 'Package number 1', '100 for 5 lessons')
SELECT * FROM Package

INSERT INTO Student(FirstName,LastName,SchoolName,StudentStatus, StudentEmail, StudentPass,StudentLanguage, DateOfTheory,TeacherID,  LengthOfLesson, DrivingTechnic, Gender, StudentId, DateOfBirth, PhoneNumber, CurrentLessonNum, InternalTestDone, StudentAddress, PackageID)
VALUES('Ori', 'Geva', 'Ramon',2, 'a@gmail.com', '123a', 'hebrow', '11-FEB-2025',1, 40, 'autumat', 'male', '000000000', '11-FEB-2007', '0000000000', 0,  1, 'golsa', 1)
SELECT * FROM Student
SELECT * FROM Teacher
SELECT * FROM Statuses




-- Create a login for the admin user
CREATE LOGIN [DrivingSchoolAdminLogin] WITH PASSWORD = 'thePassword';
Go

-- Create a user in the DrivingSchoolDB database for the login
CREATE USER [DrivingSchoolAdminUser] FOR LOGIN [DrivingSchoolAdminLogin];
Go

SELECT * FROM Manager



-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [DrivingSchoolAdminUser];
Go

--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=DrivingSchoolDB;User ID=DrivingSchoolAdminLogin;Password=thePassword;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context DrivingSchoolDbContext -DataAnnotations –force