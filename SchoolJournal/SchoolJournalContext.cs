using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SchoolJournal
{
    public partial class SchoolJournalContext : DbContext
    {
        public SchoolJournalContext()
        {
        }

        public SchoolJournalContext(DbContextOptions<SchoolJournalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<LessonTime> LessonTimes { get; set; }
        public virtual DbSet<Mark> Marks { get; set; }
        public virtual DbSet<Progress> Progresses { get; set; }
        public virtual DbSet<SchoolYear> SchoolYears { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-2OMG8RT;Database=SchoolJournal;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class");

                entity.Property(e => e.RecruitmentDate).HasColumnType("date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Journal>(entity =>
            {
                entity.HasKey(e => new { e.FkTeacher, e.FkSubject, e.FkClass, e.FkSchoolYear })
                    .HasName("PK_SubjectForTeacher");

                entity.ToTable("Journal");

                entity.Property(e => e.FkTeacher).HasColumnName("FK_Teacher");

                entity.Property(e => e.FkSubject).HasColumnName("FK_Subject");

                entity.Property(e => e.FkClass).HasColumnName("FK_Class");

                entity.Property(e => e.FkSchoolYear).HasColumnName("FK_SchoolYear");

                entity.HasOne(d => d.FkClassNavigation)
                    .WithMany(p => p.Journals)
                    .HasForeignKey(d => d.FkClass)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassSubjectForTeacher");

                entity.HasOne(d => d.FkSchoolYearNavigation)
                    .WithMany(p => p.Journals)
                    .HasForeignKey(d => d.FkSchoolYear)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SchoolYearSubjectForTeacher");

                entity.HasOne(d => d.FkSubjectNavigation)
                    .WithMany(p => p.Journals)
                    .HasForeignKey(d => d.FkSubject)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectSubjectForTeacher");

                entity.HasOne(d => d.FkTeacherNavigation)
                    .WithMany(p => p.Journals)
                    .HasForeignKey(d => d.FkTeacher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeacherSubjectForTeacher");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lesson");

                entity.Property(e => e.FkClass).HasColumnName("FK_Class");

                entity.Property(e => e.FkLessonTime).HasColumnName("FK_LessonTime");

                entity.Property(e => e.FkSchoolYear).HasColumnName("FK_SchoolYear");

                entity.Property(e => e.FkSubject).HasColumnName("FK_Subject");

                entity.Property(e => e.FkTeacher).HasColumnName("FK_Teacher");

                entity.Property(e => e.Homework)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasDefaultValueSql("(' ')");

                entity.Property(e => e.LessonDate).HasColumnType("date");

                entity.Property(e => e.Theme)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkLessonTimeNavigation)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.FkLessonTime)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LessonTimeLesson");

                entity.HasOne(d => d.Fk)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => new { d.FkTeacher, d.FkSubject, d.FkClass, d.FkSchoolYear })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubjectForTeacherLesson");
            });

            modelBuilder.Entity<LessonTime>(entity =>
            {
                entity.ToTable("LessonTime");

                entity.Property(e => e.EndTime)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.StartTime)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Mark>(entity =>
            {
                entity.ToTable("Mark");

                entity.Property(e => e.MarkValue)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Progress>(entity =>
            {
                entity.HasKey(e => new { e.FkStudent, e.FkLesson, e.FkMark });

                entity.ToTable("Progress");

                entity.Property(e => e.FkStudent).HasColumnName("FK_Student");

                entity.Property(e => e.FkLesson).HasColumnName("FK_Lesson");

                entity.Property(e => e.FkMark).HasColumnName("FK_Mark");

                entity.HasOne(d => d.FkLessonNavigation)
                    .WithMany(p => p.Progresses)
                    .HasForeignKey(d => d.FkLesson)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LessonProgress");

                entity.HasOne(d => d.FkMarkNavigation)
                    .WithMany(p => p.Progresses)
                    .HasForeignKey(d => d.FkMark)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MarkProgress");

                entity.HasOne(d => d.FkStudentNavigation)
                    .WithMany(p => p.Progresses)
                    .HasForeignKey(d => d.FkStudent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentProgress");
            });

            modelBuilder.Entity<SchoolYear>(entity =>
            {
                entity.ToTable("SchoolYear");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.FkClass).HasColumnName("FK_Class");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Middlename)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkClassNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.FkClass)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClassStudent");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");

                entity.Property(e => e.FireDate).HasColumnType("date");

                entity.Property(e => e.HireDate).HasColumnType("date");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Middlename)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
