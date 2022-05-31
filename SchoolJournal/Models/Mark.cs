using System.Collections.Generic;

namespace SchoolJournal.Data.Models
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
