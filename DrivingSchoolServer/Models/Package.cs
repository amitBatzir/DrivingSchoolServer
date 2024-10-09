using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

[Table("Package")]
public partial class Package
{
    [Key]
    [Column("PackageID")]
    public int PackageId { get; set; }

    [Column("ManagerID")]
    public int ManagerId { get; set; }

    [StringLength(100)]
    public string Title { get; set; } = null!;

    [StringLength(300)]
    public string TheText { get; set; } = null!;

    [ForeignKey("ManagerId")]
    [InverseProperty("Packages")]
    public virtual Manager Manager { get; set; } = null!;

    [InverseProperty("Package")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
