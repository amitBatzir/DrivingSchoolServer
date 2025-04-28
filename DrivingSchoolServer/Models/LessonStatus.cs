using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

public partial class LessonStatus
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [StringLength(50)]
    public string StatusDescription { get; set; } = null!;

    [InverseProperty("Status")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
