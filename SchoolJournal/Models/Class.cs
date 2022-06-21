using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolJournal.Models
{
    public partial class Class
    {
        public Class()
        {
            Journals = new HashSet<Journal>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime RecruitmentDate { get; set; }

        public virtual ICollection<Journal> Journals { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        public static string GetClassTitleById(SchoolJournalContext db, int classId) 
        {
            return db.Classes.Where(c => c.Id == classId).Select(c => c.Title).FirstOrDefault().ToString();
        }
        public static List<SelectListItem> GetFkClassSelectList(SchoolJournalContext db) 
        {
            var classes = db.Classes;
            List<SelectListItem> fkClasses = new List<SelectListItem>();
            fkClasses.Add(new SelectListItem { Value = "0", Text = "Оберіть клас" });
            foreach (Class c in classes)
            {
                fkClasses.Add(new SelectListItem { Value = c.Id.ToString(), Text = c.Title });
            }
            return fkClasses;
        }
    }
}
