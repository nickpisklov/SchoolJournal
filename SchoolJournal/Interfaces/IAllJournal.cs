using SchoolJournal.Models;
using System.Collections.Generic;

namespace SchoolJournal.Interfaces
{
    public interface IAllJournal
    {
        IEnumerable<Journal> Journals { get; }
        Journal getObjectJournal(int JournalId);
    }
}
