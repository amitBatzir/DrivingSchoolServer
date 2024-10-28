using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DrivingSchoolServer.Models;

namespace DrivingSchoolServer.DTO
{
    public class TeacherSchedule
    {
        public int ScheduleId { get; set; }
        public string DayOfSchedule { get; set; } = null!;
        public string Beginning { get; set; } = null!;
        public string LessonLength { get; set; } = null!;
        public string Ending { get; set; } = null!;
        public int TeacherId { get; set; }
        public TeacherSchedule() { }
        public TeacherSchedule(Models.TeacherSchedule ts)
        {
            ScheduleId = ts.ScheduleId;
            DayOfSchedule = ts.DayOfSchedule;
            Beginning = ts.Beginning;
            LessonLength = ts.LessonLength;
            Ending = ts.Ending;
            TeacherId = ts.TeacherId;
        }

        public Models.TeacherSchedule GetModel()
        {
            Models.TeacherSchedule ts = new Models.TeacherSchedule();
            ts.ScheduleId = ScheduleId;
            ts.DayOfSchedule = DayOfSchedule;
            ts.Beginning = Beginning;
            ts.LessonLength = LessonLength;
            ts.Ending = Ending;
            ts.TeacherId = TeacherId;
            return ts;
        }

    }
}
