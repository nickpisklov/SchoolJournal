using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolJournal
{
    public partial class Mark
    {
        public Mark()
        {
            Progresses = new HashSet<Progress>();
        }

        public int Id { get; set; }
        public string MarkValue { get; set; }

        public virtual ICollection<Progress> Progresses { get; set; }
    }
}
