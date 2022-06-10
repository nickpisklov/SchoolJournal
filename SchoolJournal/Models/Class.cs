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
    }
}
