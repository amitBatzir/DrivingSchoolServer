using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

[Table("Manager")]
[Index("ManagerEmail", Name = "UQ__Manager__351A32D72732E34E", IsUnique = true)]
public partial class Manager
{
    [Key]
    [Column("UserManagerID")]
    public int UserManagerId { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(50)]
    public string ManagerEmail { get; set; } = null!;

    [StringLength(50)]
    public string ManagerPass { get; set; } = null!;

    public int ManagerStatus { get; set; }

    [Column("TeacherID")]
    [StringLength(50)]
    public string TeacherId { get; set; } = null!;

    [StringLength(50)]
    public string SchoolAddress { get; set; } = null!;

    [StringLength(50)]
    public string ManagerPhone { get; set; } = null!;

    [StringLength(50)]
    public string SchoolPhone { get; set; } = null!;

    [InverseProperty("Teacher")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [ForeignKey("ManagerStatus")]
    [InverseProperty("Managers")]
    public virtual Status ManagerStatusNavigation { get; set; } = null!;

    [InverseProperty("Manager")]
    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();

    [InverseProperty("Manager")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();

    [InverseProperty("Manager")]
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
