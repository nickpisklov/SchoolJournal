using Microsoft.AspNetCore.Mvc;
using SchoolJournal.Data.Interfaces;

namespace SchoolJournal.Controllers
{
    public class JournalsController : Controller
    {
        private readonly IAllJournal _allJournal;
        private readonly IJournalSubject _journalSubject;

        public JournalsController(IAllJournal allJournal, IJournalSubject journalSubject)
        {
            _allJournal = allJournal;
            _journalSubject = journalSubject;
        }

        public ViewResult List()
        {
            var journal = _allJournal.Journals;
            return View(journal);
        }
    }
}
