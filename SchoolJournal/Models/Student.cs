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
        public static Student GetStudentById(SchoolJournalContext db, int id) 
        {
            return db.Students.Where(s => s.Id == id).FirstOrDefault();
        }
    }
}
