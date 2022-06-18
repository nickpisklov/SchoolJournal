using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SchoolJournal.Models
{
    public partial class Progress
    {
        public int FkStudent { get; set; }
        public int FkLesson { get; set; }
        public int FkMark { get; set; }

        public virtual Lesson FkLessonNavigation { get; set; }
        public virtual Mark FkMarkNavigation { get; set; }
        public virtual Student FkStudentNavigation { get; set; }

        public Progress(int lessonId, int studentId) 
        {
            FkLesson = Convert.ToInt32(lessonId);
            FkStudent = Convert.ToInt32(studentId);
            FkMark = 0;
        }
        public Progress() { }

        public static Progress GetProgressByStudentAndLessonId(SchoolJournalContext db, int studentId, int lessonId) 
        {
            return db.Progresses.Where(p => p.FkLesson == lessonId && p.FkStudent == studentId).FirstOrDefault();
        }
        public void AddToDbWithDependencies(SchoolJournalContext db, Progress progress) 
        {
            var student = db.Students.Find(progress.FkStudent);
            var lesson = db.Lessons.Find(progress.FkLesson);
            student.Progresses.Add(progress);
            lesson.Progresses.Add(progress);
            db.Progresses.Add(progress);
            db.SaveChanges();
        }
        public void ModifyDbRecord(SchoolJournalContext db, Progress progress, Progress progressCheck) 
        {
            db.Progresses.Remove(progressCheck);
            db.SaveChanges();
            db.Entry(progress).State = EntityState.Added;
            db.SaveChanges();
        }
    }
}
