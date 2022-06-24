using System.Collections.Generic;
using System.Linq;

namespace SchoolJournal.Models
{
    public partial class Journal
    {
        public Journal()
        {
            Lessons = new HashSet<Lesson>();
        }

        public int FkTeacher { get; set; }
        public int FkSubject { get; set; }
        public int FkClass { get; set; }
        public int FkSchoolYear { get; set; }

        public virtual Class FkClassNavigation { get; set; }
        public virtual SchoolYear FkSchoolYearNavigation { get; set; }
        public virtual Subject FkSubjectNavigation { get; set; }
        public virtual Teacher FkTeacherNavigation { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }

        public void AddJournalWithDependencies(SchoolJournalContext db) 
        {
            var classDependencies = db.Classes.Find(FkClass);
            classDependencies.Journals.Add(this);
            var subjectDependencies = db.Subjects.Find(FkSubject);
            subjectDependencies.Journals.Add(this);
            var yearDependencies = db.SchoolYears.Find(FkSchoolYear);
            yearDependencies.Journals.Add(this);
            var teacherDependencies = db.Teachers.Find(FkTeacher);
            teacherDependencies.Journals.Add(this);
            db.Journals.Add(this);
            db.SaveChanges();
        }
        public bool IsJournalExists(SchoolJournalContext db) 
        {
            var journal = db.Journals.Where(j => j.FkClass == FkClass && j.FkSchoolYear == FkSchoolYear
            && j.FkSubject == FkSubject && j.FkTeacher == FkTeacher).FirstOrDefault();
            if (journal == null)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }
    }
}
