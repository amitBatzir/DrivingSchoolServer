﻿using System;
using System.Collections.Generic;
using DrivingSchoolServer.DTO;
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

    public Teacher? GetTeacherWithLessons(string email)
    {
        return this.Teachers.Include(t => t.Lessons).ThenInclude(l => l.Student).Where(t => t.TeacherEmail == email).FirstOrDefault();
    }
    

    public Teacher? GetTeacherWithLessons(int  id)
    {
        return this.Teachers.Include(t => t.Lessons).ThenInclude(l => l.Student).Where(t => t.UserTeacherId == id).FirstOrDefault();
    }
    public Lesson? GetLessonWithStudent(int id)
    {
        return this.Lessons.Include(l => l.Student).Where(l=> l.LessonId == id).FirstOrDefault();
    }

    public Manager? GetManager(string email)
    {
        return this.Managers.Where(m => m.ManagerEmail == email).FirstOrDefault();
    }
    public Manager? GetSchoolName(string schoolName)
    {
        return this.Managers.Where(m => m.SchoolName == schoolName).FirstOrDefault();

    }
}
