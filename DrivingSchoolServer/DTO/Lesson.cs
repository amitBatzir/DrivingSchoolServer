    using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DrivingSchoolServer.DTO
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public DateTime DateOfLesson { get; set; }  
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public string PickUpLoc { get; set; } = null!;
        public string DropOffLoc { get; set; } = null!;
        public int StatusId { get; set; }
        public Student? Student { get; set; }
        public Lesson() { }
        public Lesson(Models.Lesson l)
        {
            LessonId = l.LessonId;
            DateOfLesson = l.DateOfLesson;
            StudentId = l.StudentId;
            TeacherId = l.TeacherId;
            PickUpLoc = l.PickUpLoc;
            DropOffLoc = l.DropOffLoc;
            StatusId = l.StatusId;  
            if (l.Student != null)
            {
                Student = new Student(l.Student);
            }
        }

        public Models.Lesson GetModel()
        {
            Models.Lesson l = new Models.Lesson();
            l.LessonId = LessonId;
            l.DateOfLesson = DateOfLesson;
            l.StudentId = StudentId;
            l.TeacherId = TeacherId;
            l.PickUpLoc = PickUpLoc;
            l.DropOffLoc = DropOffLoc;
            l.StatusId = StatusId;
            return l;
        }

    }

    public class LessonStatuses
    {
        public int StatusId { get; set; }

        public string StatusDescription { get; set; } = null!;
    }
}
