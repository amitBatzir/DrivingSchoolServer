using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

[Table("Student")]
[Index("StudentEmail", Name = "UQ__Student__3569CFDB0577D45E", IsUnique = true)]
public partial class Student
{
    [Key]
    [Column("UserStudentID")]
    public int UserStudentId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string SchoolName { get; set; } = null!;

    public int StudentStatus { get; set; }

    [StringLength(50)]
    public string StudentEmail { get; set; } = null!;

    [StringLength(50)]
    public string StudentPass { get; set; } = null!;

    [StringLength(50)]
    public string StudentLanguage { get; set; } = null!;

    public DateTime DateOfTheory { get; set; }

    public int LengthOfLesson { get; set; }

    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [StringLength(50)]
    public string DrivingTechnic { get; set; } = null!;

    [StringLength(50)]
    public string Gender { get; set; } = null!;

    [StringLength(50)]
    public string StudentId { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    [StringLength(10)]
    public string PhoneNumber { get; set; } = null!;

    public int CurrentLessonNum { get; set; }

    public bool InternalTestDone { get; set; }

    [StringLength(50)]
    public string StudentAddress { get; set; } = null!;

    [StringLength(500)]
    public string? ProfilePic { get; set; }

    [Column("PackageID")]
    public int PackageId { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("Student")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    [ForeignKey("PackageId")]
    [InverseProperty("Students")]
    public virtual Package Package { get; set; } = null!;

    [ForeignKey("StudentStatus")]
    [InverseProperty("Students")]
    public virtual Status StudentStatusNavigation { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("Students")]
    public virtual Teacher Teacher { get; set; } = null!;

    [InverseProperty("Student")]
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
