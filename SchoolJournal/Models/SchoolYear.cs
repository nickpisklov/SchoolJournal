using System;
using System.Collections.Generic;

namespace SchoolJournal.Models
{
    public partial class SchoolYear
    {
        public SchoolYear()
        {
            Journals = new HashSet<Journal>();
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Journal> Journals { get; set; }
    }
}
