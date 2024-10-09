using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

public partial class Test
{
    [Key]
    [Column("TestID")]
    public int TestId { get; set; }

    [Column("StudentID")]
    public int StudentId { get; set; }

    [Column("ManagerID")]
    public int ManagerId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateOfTest { get; set; }

    public bool PassedOrNot { get; set; }

    [Column("comments")]
    [StringLength(500)]
    public string Comments { get; set; } = null!;

    [ForeignKey("ManagerId")]
    [InverseProperty("Tests")]
    public virtual Manager Manager { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("Tests")]
    public virtual Student Student { get; set; } = null!;
}
