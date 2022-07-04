using Microsoft.AspNetCore.Mvc.Rendering;
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
        public static Teacher GetTeacherByLogin(SchoolJournalContext db, string login)
        {
            return db.Teachers.Where(s => s.Login == login).FirstOrDefault();
        }
        public static List<SelectListItem> GetFkTeachersSelectList(SchoolJournalContext db) 
        {
            var teachers = db.Teachers.Where(t => t.FireDate == null);
            List<SelectListItem> fkTeachers = new List<SelectListItem>();
            fkTeachers.Add(new SelectListItem { Value = "0", Text = "Оберіть вчителя" });
            foreach (Teacher t in teachers)
            {
                fkTeachers.Add(new SelectListItem
                { Value = t.Id.ToString(), Text = $"{t.Surname} {t.Name} {t.Middlename}" });
            }
            return fkTeachers;
        }
    }
}
