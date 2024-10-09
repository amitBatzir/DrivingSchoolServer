using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

[Keyless]
[Table("HomePage")]
public partial class HomePage
{
    [StringLength(2000)]
    public string HomePageText { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime UpdateTime { get; set; }
}
