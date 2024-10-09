using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

[Table("Comment")]
public partial class Comment
{
    [Key]
    [Column("CommentID")]
    public int CommentId { get; set; }

    [Column("StudentID")]
    public int StudentId { get; set; }

    [Column("TeacherID")]
    public int TeacherId { get; set; }

    [StringLength(200)]
    public string TheText { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("Comments")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("Comments")]
    public virtual Manager Teacher { get; set; } = null!;
}
