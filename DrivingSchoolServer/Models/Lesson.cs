using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

[Table("Lesson")]
public partial class Lesson
{
    [Key]
    [Column("LessonID")]
    public int LessonId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateOfLesson { get; set; }

    [Column("StudentID")]
    public int StudentId { get; set; }

    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [StringLength(50)]
    public string PickUpLoc { get; set; } = null!;

    [StringLength(50)]
    public string DropOffLoc { get; set; } = null!;

    public int StatusId { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Lessons")]
    public virtual LessonStatus Status { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("Lessons")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("Lessons")]
    public virtual Teacher Teacher { get; set; } = null!;
}
