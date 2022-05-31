using SchoolJournal.Data.Models;
using System.Collections.Generic;

namespace SchoolJournal.Data.Interfaces
{
    public interface IJournalSubject
    {
        IEnumerable<Subject> AllSubjects { get; }
    }
}
