using SchoolJournal.Models;
using System.Collections.Generic;

namespace SchoolJournal.Interfaces
{
    public interface IJournalSubject
    {
        IEnumerable<Subject> AllSubjects { get; }
    }
}
