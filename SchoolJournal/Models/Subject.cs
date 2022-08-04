using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace SchoolJournal.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Journals = new HashSet<Journal>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Journal> Journals { get; set; }

        public static string GetSubjectTitleById(SchoolJournalContext db ,int subjectId) 
        {
            return db.Subjects.Where(s => s.Id == subjectId).Select(s => s.Title).FirstOrDefault().ToString();
        }
    }
}
