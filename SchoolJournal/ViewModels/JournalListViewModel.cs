using SchoolJournal.Models;
using System.Collections.Generic;

namespace SchoolJournal.ViewModels
{
    public class JournalListViewModel
    {
        public IEnumerable<Journal> allJournals { get; set; }
        public string currSubject { get; set; }
    }
}
