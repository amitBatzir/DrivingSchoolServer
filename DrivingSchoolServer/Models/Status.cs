using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

public partial class S
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [StringLength(50)]
    public string StatusDescription { get; set; } = null!;

    [InverseProperty("ManagerStatusNavigation")]
    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();

    [InverseProperty("StudentStatusNavigation")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [InverseProperty("TeacherStatusNavigation")]
    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
