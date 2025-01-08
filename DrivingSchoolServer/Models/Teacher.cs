using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

[Table("Teacher")]
[Index("TeacherEmail", Name = "UQ__Teacher__4A75D54C9F6E66C1", IsUnique = true)]
public partial class Teacher
{
    [Key]
    [Column("UserTeacherID")]
    public int UserTeacherId { get; set; }

    [StringLength(50)]
    public string SchoolName { get; set; } = null!;

    [StringLength(50)]
    public string TeacherEmail { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string TeacherPass { get; set; } = null!;

    public int TeacherStatus { get; set; }

    [Column("TeacherID")]
    [StringLength(50)]
    public string TeacherId { get; set; } = null!;

    [StringLength(50)]
    public string WayToPay { get; set; } = null!;

    [StringLength(50)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(50)]
    public string Gender { get; set; } = null!;

    [StringLength(50)]
    public string? ProfilePic { get; set; }

    [StringLength(50)]
    public string DrivingTechnic { get; set; } = null!;

    [Column("ManagerID")]
    public int ManagerId { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    [ForeignKey("ManagerId")]
    [InverseProperty("Teachers")]
    public virtual Manager Manager { get; set; } = null!;

    [InverseProperty("Teacher")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TeacherSchedule> TeacherSchedules { get; set; } = new List<TeacherSchedule>();

    [ForeignKey("TeacherStatus")]
    [InverseProperty("Teachers")]
    public virtual Status TeacherStatusNavigation { get; set; } = null!;
}
