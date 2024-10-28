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
        public bool DidExist { get; set; }
        public Lesson() { }
        public Lesson(Models.Lesson l)
        {
            LessonId = l.LessonId;
            DateOfLesson = l.DateOfLesson;
            StudentId = l.StudentId;
            TeacherId = l.TeacherId;
            PickUpLoc = l.PickUpLoc;
            DropOffLoc = l.DropOffLoc;
            DidExist = l.DidExist;           
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
            l.DidExist = DidExist;
            return l;
        }

    }
}
