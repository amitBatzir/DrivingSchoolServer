using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

[Table("TeacherSchedule")]
public partial class TeacherSchedule
{
    [Key]
    [Column("ScheduleID")]
    public int ScheduleId { get; set; }

    [StringLength(15)]
    public string DayOfSchedule { get; set; } = null!;

    [StringLength(15)]
    public string Beginning { get; set; } = null!;

    [StringLength(2)]
    public string LessonLength { get; set; } = null!;

    [StringLength(15)]
    public string Ending { get; set; } = null!;

    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [ForeignKey("TeacherId")]
    [InverseProperty("TeacherSchedules")]
    public virtual Teacher Teacher { get; set; } = null!;
}
