﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrivingSchoolServer.DTO
{
    public class Student
    {
        public int UserStudentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string SchoolName { get; set; } = null!;
        public int StudentStatus { get; set; }
        public string StudentEmail { get; set; } = null!;
        public string StudentPass { get; set; } = null!;
        public string StudentLanguage { get; set; } = null!;
        public bool DoneTheoryTest { get; set; }
        public DateTime DateOfTheory { get; set; }
        public int LengthOfLesson { get; set; }
        public int TeacherId { get; set; }
        public string DrivingTechnic { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public int CurrentLessonNum { get; set; } 
        public bool InternalTestDone { get; set; }
        public string StudentAddress { get; set; } = null!;
        public string? ProfilePic { get; set; } = null!;
        public int PackageId { get; set; }
        public Student() { }
        public Student(Models.Student st)
        {
            UserStudentId = st.UserStudentId;
            FirstName = st.FirstName;
            LastName = st.LastName;
            StudentStatus = st.StudentStatus;
            StudentEmail = st.StudentEmail;
            StudentPass = st.StudentPass;
            StudentLanguage = st.StudentLanguage;
            DateOfTheory = st.DateOfTheory;
            LengthOfLesson= st.LengthOfLesson;
            TeacherId = st.TeacherId;
            DrivingTechnic = st.DrivingTechnic;
            Gender = st.Gender;
            StudentId = st.StudentId;
            DateOfBirth = st.DateOfBirth;
            PhoneNumber = st.PhoneNumber;
            CurrentLessonNum = st.CurrentLessonNum;
            InternalTestDone = st.InternalTestDone;
            StudentAddress = st.StudentAddress;
            st.ProfilePic = ProfilePic;
            PackageId = st.PackageId;
        }

        public Models.Student GetModel()
        {
            Models.Student st = new Models.Student();
            st.UserStudentId = UserStudentId;
            st.FirstName = FirstName;
            st.LastName = LastName;
            st.StudentStatus = StudentStatus;
            st.StudentEmail = StudentEmail;
            st.StudentPass = StudentPass;
            st.StudentLanguage = StudentLanguage;
            st.DateOfTheory = DateOfTheory;
            st.LengthOfLesson = LengthOfLesson;
            st.TeacherId = TeacherId;
            st.DrivingTechnic = DrivingTechnic;
            st.Gender = Gender;
            st.StudentId = StudentId;
            st.DateOfBirth = DateOfBirth;
            st.PhoneNumber = PhoneNumber;
            st.CurrentLessonNum = CurrentLessonNum;
            st.InternalTestDone = InternalTestDone;
            st.StudentAddress = StudentAddress;
            st.ProfilePic = ProfilePic;
            st.PackageId = PackageId;
            st.SchoolName = SchoolName;
            return st;
        }

    }
}
