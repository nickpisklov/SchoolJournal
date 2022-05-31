using System;
using System.Collections.Generic;

namespace SchoolJournal.Data.Models
namespace SchoolJournal
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
    }
}
