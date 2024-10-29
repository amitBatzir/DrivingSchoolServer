using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

public partial class DrivingSchoolDbContext : DbContext
{
    public Student? GetStudent(string email)
    {
        return this.Students.Where(s => s.StudentEmail == email).FirstOrDefault();
    }
    public Teacher? GetTeacher(string email)
    {
        return this.Teachers.Where(t => t.TeacherEmail == email).FirstOrDefault();
    }
    public Manager? GetManager(string email)
    {
        return this.Managers.Where(m => m.ManagerEmail == email).FirstOrDefault();
    }
}
