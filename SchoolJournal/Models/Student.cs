using System.Collections.Generic;
using System.Linq;

namespace SchoolJournal.Models
{
    public partial class Student
    {
        public Student()
        {
            Progresses = new HashSet<Progress>();
        }

        public int Id { get; set; }
        public int FkClass { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }

        public virtual Class FkClassNavigation { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; }

        public static IQueryable<Student>GetStudentsByClass(SchoolJournalContext db, int classId) 
        {
            return db.Students.Where(s => s.FkClass == classId).OrderBy(s => s.Surname);
        }
        public void AddStudentWithDependencies(SchoolJournalContext db) 
        {
            var classDependencies = db.Classes.Find(FkClass);
            classDependencies.Students.Add(this);
            db.Students.Add(this);
            db.SaveChanges();
        }
        public bool IsLoginEcxist(SchoolJournalContext db) 
        {
            var t = db.Teachers.Where(t => t.Login == Login).FirstOrDefault();
            var s = db.Students.Where(s => s.Login == Login).FirstOrDefault();
            var a = db.Admins.Where(a => a.Login == Login).FirstOrDefault();
            if (t == null && s == null && a == null)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }
        public static Student GetStudentById(SchoolJournalContext db, int id)
        {
            return db.Students.Where(s => s.Id == id).FirstOrDefault();
        }
        public static Student GetStudentByLogin(SchoolJournalContext db, string login)
        {
            return db.Students.Where(s => s.Login == login).FirstOrDefault();
        }
    }
}
