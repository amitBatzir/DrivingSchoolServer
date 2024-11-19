Use master
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
TeacherID nvarchar(50) Not Null,
SchoolAddress nvarchar(50) Not Null,
ManagerPhone nvarchar(50) Not Null,
SchoolPhone nvarchar(50) Not Null,
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
ProfilePic nvarchar(50) Not Null,
ManagerID int Not Null Foreign Key References Manager(UserManagerID),
)

Create table Student
(
UserStudentID int Primary Key Identity(1,1),
FirstName nvarchar(50) Not Null,
LastName nvarchar(50) Not Null,
StudentStatus int Not Null Foreign Key References Statuses(StatusID),
StudentEmail nvarchar(50) Unique Not Null,
StudentPass nvarchar(50) Not Null,
SchoolName nvarchar(50) Not Null,
StudentLanguage nvarchar(50) Not Null,
DoneTheoryTest bit Not Null Default 1,
DateOfTheory Date Not Null,
LengthOfLesson int Not Null,
HaveDocuments bit Not Null Default 1,
TeacherID int Not Null Foreign Key References Teacher(UserTeacherID),
DrivingTechnic nvarchar(50) Not Null,
Gender nvarchar(50) Not Null,
StudentId nvarchar(50) Not Null,
DateOfBirth Date Not Null,
PhoneNumber nvarchar(10) Not Null,
CurrentLessonNum nvarchar (4) Not Null,
InternalTestDone bit Not Null Default 1,
StudentAddress nvarchar(50) Not Null,
ProfilePic nvarchar(50) Not Null,
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
VALUES('Approved')
SELECT * FROM Statuses
INSERT INTO Statuses(StatusDescription)
VALUES('Pending')
INSERT INTO Statuses(StatusDescription)
VALUES('Declined')

INSERT INTO Manager(FirstName, LastName, ManagerEmail, ManagerPass, ManagerStatus, TeacherID, SchoolAddress,  ManagerPhone, SchoolPhone, SchoolName)
VALUES('Itzik', 'Rotem', 'Rotem@gmail.com', '123', 1, '217389065', 'Golda Meir', '0537786549', '03456789', 'Ramon')
SELECT * FROM Manager

-- Create a login for the admin user
CREATE LOGIN [DrivingSchoolAdminLogin] WITH PASSWORD = 'thePassword';
Go

-- Create a user in the DrivingSchoolDB database for the login
CREATE USER [DrivingSchoolAdminUser] FOR LOGIN [DrivingSchoolAdminLogin];
Go

Select * from  Manager;
Select * from  Statuses;


-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [DrivingSchoolAdminUser];
Go

--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=DrivingSchoolDB;User ID=DrivingSchoolAdminLogin;Password=thePassword;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context DrivingSchoolDbContext -DataAnnotations –force