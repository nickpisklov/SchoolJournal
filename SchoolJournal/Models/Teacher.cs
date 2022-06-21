using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolJournal.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Journals = new HashSet<Journal>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? FireDate { get; set; }

        public virtual ICollection<Journal> Journals { get; set; }

        public bool IsLoginEcxist(SchoolJournalContext db, Teacher teacher)
        {
            var t = db.Teachers.Where(s => s.Login == teacher.Login).FirstOrDefault();
            if (t == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static Teacher GetTeacherByLogin(SchoolJournalContext db, string login)
        {
            return db.Teachers.Where(s => s.Login == login).FirstOrDefault();
        }
    }
}
