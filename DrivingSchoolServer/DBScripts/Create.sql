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

Create Table LessonStatuses 
(
StatusID int Primary Key,
StatusDescription nvarchar(50) Not Null,
)

INSERT INTO LessonStatuses VALUES (1, N'Pending')
INSERT INTO LessonStatuses VALUES (2, N'Approved')
INSERT INTO LessonStatuses VALUES (3, N'Done')
INSERT INTO LessonStatuses VALUES (4, N'Declined')
INSERT INTO LessonStatuses VALUES (5, N'Canceled')

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
DateOfTheory DateTime Not Null,
LengthOfLesson int Not Null,
TeacherID int Not Null Foreign Key References Teacher(UserTeacherID),
DrivingTechnic nvarchar(50) Not Null,
Gender nvarchar(50) Not Null,
StudentId nvarchar(50) Not Null,
DateOfBirth DateTime Not Null,
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
StatusId int Not Null Foreign Key References LessonStatuses(StatusId),

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
SELECT * FROM Statuses


INSERT INTO Manager(FirstName, LastName, ManagerEmail, ManagerPass, ManagerStatus,ManagerID, SchoolAddress,  ManagerPhone, SchoolPhone, SchoolName)
VALUES('Itzik', 'Rotem', 'Rotem@gmail.com', '123', 2, '234567890', 'Golda Meir', '0537786549', '03456789', 'Ramon')
INSERT INTO Manager(FirstName, LastName, ManagerEmail, ManagerPass, ManagerStatus,ManagerID, SchoolAddress,  ManagerPhone, SchoolPhone, SchoolName)
VALUES('Shahar', 'Batzir', 'S@gmail.com', 's123', 2, '217328566', 'Golda Meir', '0537786549', '03456789', 'Tov')
INSERT INTO Manager(FirstName, LastName, ManagerEmail, ManagerPass, ManagerStatus,ManagerID, SchoolAddress,  ManagerPhone, SchoolPhone, SchoolName)
VALUES('Amit', 'Batzir', 'Amit@driverseat.com', 'manager12', 2, '217328574', 'Golda Meir', '0537786549', '03456789', 'App')
SELECT * FROM Manager


INSERT INTO Teacher(SchoolName, TeacherEmail, FirstName, LastName, TeacherPass,TeacherStatus, TeacherID,  WayToPay, PhoneNumber, Gender, ManagerID, DrivingTechnic)
VALUES('Ramon', 'Marom@gmail.com', 'Marom', 'Hai', 'm123',2, '111111111', 'Cash', '000000000', 'female', 1, 'Autumat')
INSERT INTO Teacher(SchoolName, TeacherEmail, FirstName, LastName, TeacherPass,TeacherStatus, TeacherID,  WayToPay, PhoneNumber, Gender, ManagerID, DrivingTechnic)
VALUES('Ramon', 'Shahar@gmail.com', 'Shahar', 'Batzir', 's123',1 ,'2222222', 'Cash', '000000000', 'female', 1, 'Autumat')
INSERT INTO Teacher(SchoolName, TeacherEmail, FirstName, LastName, TeacherPass,TeacherStatus, TeacherID,  WayToPay, PhoneNumber, Gender, ManagerID, DrivingTechnic)
VALUES('Ramon', 'Gal@gmail.com', 'Gal', 'Klug', 'g123',1 ,'2223222', 'Cash', '000000000', 'female', 1, 'Autumat')
SELECT * FROM Teacher

INSERT INTO Package(ManagerID, Title, TheText)
VALUES(1, 'Package number 1', '100 for 5 lessons')
INSERT INTO Package(ManagerID, Title, TheText)
VALUES(1, 'Package number 2', '200 for 5 lessons')
INSERT INTO Package(ManagerID, Title, TheText)
VALUES(1, 'Package number 3', '1 for 5 lessons')
SELECT * FROM Package

INSERT INTO Student(FirstName,LastName,SchoolName,StudentStatus, StudentEmail, StudentPass,StudentLanguage, DateOfTheory,TeacherID,  LengthOfLesson, DrivingTechnic, Gender, StudentId, DateOfBirth, PhoneNumber, CurrentLessonNum, InternalTestDone, StudentAddress, PackageID)
VALUES('Ori', 'Geva', 'Ramon',2, 'o@gmail.com', '123o', 'hebrow', '11-FEB-2025',1, 40, 'autumat', 'male', '000000000', '11-FEB-2007', '0000000000', 0,  1, 'golsa', 1)
INSERT INTO Student(FirstName,LastName,SchoolName,StudentStatus, StudentEmail, StudentPass,StudentLanguage, DateOfTheory,TeacherID,  LengthOfLesson, DrivingTechnic, Gender, StudentId, DateOfBirth, PhoneNumber, CurrentLessonNum, InternalTestDone, StudentAddress, PackageID)
VALUES('Maayan', 'Kisluk', 'Ramon',1, 'Maayan@gmail.com', 'Maayan123', 'hebrow', '11-FEB-2025',1, 10, 'autumat', 'Female', '000000000', '11-FEB-2007', '0000000000', 0,  1, 'herzog', 1)
INSERT INTO Student(FirstName,LastName,SchoolName,StudentStatus, StudentEmail, StudentPass,StudentLanguage, DateOfTheory,TeacherID,  LengthOfLesson, DrivingTechnic, Gender, StudentId, DateOfBirth, PhoneNumber, CurrentLessonNum, InternalTestDone, StudentAddress, PackageID)
VALUES('Roni', 'Kalfon', 'Ramon',2, 'R@gmail.com', 'R111', 'hebrow', '11-FEB-2025',2, 10, 'autumat', 'Female', '000000000', '11-FEB-2007', '0000000000', 0,  1, 'herzog', 1)
SELECT * FROM Student

INSERT INTO HomePage(HomePageText, UpdateTime)
VALUES('ניסיון אחד עמוד בית','21-APR-2025')


INSERT INTO Lesson(DateOfLesson,StudentID,TeacherID,PickUpLoc,DropOffLoc,[StatusId])
VALUES('10-JUN-2025 09:00', 1,1,'ramon','ramon',1)
INSERT INTO Lesson(DateOfLesson,StudentID,TeacherID,PickUpLoc,DropOffLoc,[StatusId])
VALUES('24-AUG-2025 10:00', 2,1,'Golda','Stav',1)
INSERT INTO Lesson(DateOfLesson,StudentID,TeacherID,PickUpLoc,DropOffLoc,[StatusId])
VALUES('5-JUL-2025 8:00', 2,1,'herzel','herzel',2)
INSERT INTO Lesson(DateOfLesson,StudentID,TeacherID,PickUpLoc,DropOffLoc,[StatusId])
VALUES('5-JUL-2025 9:00', 1,1,'golda','golda',2)
INSERT INTO Lesson(DateOfLesson,StudentID,TeacherID,PickUpLoc,DropOffLoc,[StatusId])
VALUES('12-DEC-2024 9:00', 1,1,'stav','stav',2)
INSERT INTO Lesson(DateOfLesson,StudentID,TeacherID,PickUpLoc,DropOffLoc,[StatusId])
VALUES('12-DEC-2024 9:00',2,1,'haimHerzog','haimHerzog',2)
SELECT * FROM Lesson

-- Create a login for the admin user
CREATE LOGIN [DrivingSchoolAdminLogin] WITH PASSWORD = 'thePassword';
Go

-- Create a user in the DrivingSchoolDB database for the login
CREATE USER [DrivingSchoolAdminUser] FOR LOGIN [DrivingSchoolAdminLogin];
Go




-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [DrivingSchoolAdminUser];
Go

--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=DrivingSchoolDB;User ID=DrivingSchoolAdminLogin;Password=thePassword;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context DrivingSchoolDbContext -DataAnnotations –force