using SchoolJournal.Data.Models;
using System.Collections.Generic;

namespace SchoolJournal.Data.Interfaces
{
    public interface IAllJournal
    {
        IEnumerable<Journal> Journals { get; }
        Journal getObjectJournal(int JournalId);
    }
}
