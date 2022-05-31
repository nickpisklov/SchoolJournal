using System.Collections.Generic;

namespace SchoolJournal
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
    }
}
