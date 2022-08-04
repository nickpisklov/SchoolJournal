using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SchoolJournal.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            Progresses = new HashSet<Progress>();
        }
        public int Id { get; set; }
        public int FkTeacher { get; set; }
        public int FkSubject { get; set; }
        public int FkClass { get; set; }
        public int FkSchoolYear { get; set; }
        public int FkLessonTime { get; set; }
        public DateTime LessonDate { get; set; }
        public string Theme { get; set; }
        public string Homework { get; set; }

        public virtual Journal Fk { get; set; }
        public virtual LessonTime FkLessonTimeNavigation { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; }

        public static IQueryable<Lesson> GetLessonsForClass(SchoolJournalContext db, int classId, int schoolYearId, int subjectId, int teacherId)
        {
            return db.Lessons.Where(l => l.FkClass == classId && l.FkSchoolYear == schoolYearId
                && l.FkSubject == subjectId && l.FkTeacher == teacherId).OrderBy(l => l.LessonDate);
        }
        public static IQueryable<Lesson> GetLessonsForClass(SchoolJournalContext db, int classId, int schoolYearId)
        {
            return db.Lessons.Where(l => l.FkClass == classId && l.FkSchoolYear == schoolYearId);
        }
        public static List<Lesson> GetLessonsForTeacher(SchoolJournalContext db, int teacherId) 
        {
            return db.Lessons.Where(l => l.FkTeacher == teacherId).ToList();
        }
        public static Lesson GetLessonById(SchoolJournalContext db, int lessonId) 
        {
            return db.Lessons.Where(l => l.Id == lessonId).FirstOrDefault();
        }
        public void FillLesssonProperties(string fkClass, string fkTime, string lessonDate, int year)
        {
            FkClass = Convert.ToInt32(fkClass);
            FkLessonTime = Convert.ToInt32(fkTime);
            FkSchoolYear = year;
            LessonDate = Convert.ToDateTime(lessonDate);
            Theme = " ";
            Homework = " ";
        }
        public void AddToDbWithDependencies(SchoolJournalContext db) 
        {
            var timeDep = db.LessonTimes.Find(FkLessonTime);
            timeDep.Lessons.Add(this);
            db.Lessons.Add(this);
            db.SaveChanges();
        }
        public void UpdateWithDependencies(SchoolJournalContext db, Lesson oldLesson) 
        {
            
            db.SaveChanges();

        }
    }
}
