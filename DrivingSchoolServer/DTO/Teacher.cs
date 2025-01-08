using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DrivingSchoolServer.Models;

namespace DrivingSchoolServer.DTO
{
    public class Teacher
    {
        public int UserTeacherId { get; set; }
        public string SchoolName { get; set; } = null!;
        public string TeacherEmail { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string TeacherPass { get; set; } = null!;
        public int TeacherStatus { get; set; }
        public string TeacherId { get; set; } = null!;
        public string WayToPay { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string? ProfilePic { get; set; } = null!;
        public string DrivingTechnic { get; set; } = null!;
        public int ManagerId { get; set; }
  
        public Teacher() { }
        public Teacher(Models.Teacher t)
        {
            UserTeacherId = t.UserTeacherId;
            SchoolName = t.SchoolName;
            TeacherEmail = t.TeacherEmail;
            FirstName = t.FirstName;
            LastName = t.LastName;
            TeacherPass = t.TeacherPass;
            TeacherStatus = t.TeacherStatus;
            TeacherId = t.TeacherId;
            WayToPay = t.WayToPay;
            PhoneNumber = t.PhoneNumber;
            Gender = t.Gender;
            ProfilePic = t.ProfilePic;
            DrivingTechnic = t.DrivingTechnic;
            ManagerId = t.ManagerId;

        }

        public Models.Teacher GetModel()
        {
            Models.Teacher t = new Models.Teacher();
            t.UserTeacherId = UserTeacherId;
            t.SchoolName = SchoolName;
            t.TeacherEmail = TeacherEmail;
            t.FirstName = FirstName;
            t.LastName = LastName;
            t.TeacherPass = TeacherPass;
            t.TeacherStatus = TeacherStatus;
            t.TeacherId = TeacherId;
            t.WayToPay = WayToPay;
            t.PhoneNumber = PhoneNumber;
            t.Gender = Gender;
            t.ProfilePic = ProfilePic;
            t.DrivingTechnic = DrivingTechnic;
            t.ManagerId = ManagerId;
            return t;
        }

    }
}
