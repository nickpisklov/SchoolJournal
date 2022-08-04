using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Будь ласка, введіть логін вчителя!")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть пароль вчителя!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть ім'я вчителя!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть прізвище вчителя!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть по-батькові вчителя!")]
        public string Middlename { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть дату найму вчителя!")]
        public DateTime? HireDate { get; set; }
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
        
    }
}
