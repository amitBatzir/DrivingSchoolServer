using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolServer.Models;

public partial class DrivingSchoolDbContext : DbContext
{
    public DrivingSchoolDbContext()
    {
    }

    public DrivingSchoolDbContext(DbContextOptions<DrivingSchoolDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<HomePage> HomePages { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Manager> Managers { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherSchedule> TeacherSchedules { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=DrivingSchoolDB;User ID=DrivingSchoolAdminLogin;Password=thePassword;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__C3B4DFAA0716F268");

            entity.HasOne(d => d.Student).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__Student__3A81B327");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__Teacher__3B75D760");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lesson__B084ACB0B8B6A92E");

            entity.Property(e => e.DidExist).HasDefaultValue(true);

            entity.HasOne(d => d.Student).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lesson__StudentI__45F365D3");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Lesson__TeacherI__46E78A0C");
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.HasKey(e => e.UserManagerId).HasName("PK__Manager__96A0B52DCBB7AB56");

            entity.HasOne(d => d.ManagerStatusNavigation).WithMany(p => p.Managers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manager__Manager__276EDEB3");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__Package__322035ECD6BF1BD8");

            entity.HasOne(d => d.Manager).WithMany(p => p.Packages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Package__Manager__2A4B4B5E");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Statuses__C8EE20432BAC6D8D");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.UserStudentId).HasName("PK__Student__ADF851764F228B11");

            entity.Property(e => e.DoneTheoryTest).HasDefaultValue(true);
            entity.Property(e => e.HaveDocuments).HasDefaultValue(true);
            entity.Property(e => e.InternalTestDone).HasDefaultValue(true);

            entity.HasOne(d => d.Package).WithMany(p => p.Students)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student__Package__37A5467C");

            entity.HasOne(d => d.StudentStatusNavigation).WithMany(p => p.Students)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student__Student__32E0915F");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Students)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student__Teacher__35BCFE0A");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.UserTeacherId).HasName("PK__Teacher__365C454B0AA007D8");

            entity.HasOne(d => d.Manager).WithMany(p => p.Teachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Teacher__Manager__2F10007B");

            entity.HasOne(d => d.TeacherStatusNavigation).WithMany(p => p.Teachers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Teacher__Teacher__2E1BDC42");
        });

        modelBuilder.Entity<TeacherSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__TeacherS__9C8A5B69EF681436");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TeacherSc__Teach__3E52440B");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__Tests__8CC3310099AC3A59");

            entity.Property(e => e.PassedOrNot).HasDefaultValue(true);

            entity.HasOne(d => d.Manager).WithMany(p => p.Tests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tests__ManagerID__4222D4EF");

            entity.HasOne(d => d.Student).WithMany(p => p.Tests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tests__StudentID__412EB0B6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
