using System.Collections.Generic;

namespace SchoolJournal.Data.Interfaces
{
    interface IJournalSubject
    {
        IEnumerable<Subject> AllSubjects { get; }
    }
}
