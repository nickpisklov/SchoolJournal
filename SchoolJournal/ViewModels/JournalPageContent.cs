using SchoolJournal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.ViewModels
{
    public class JournalPageContent
    {
        public Lesson LessonDetails { get; set; }
        public Student StudentDetails { get; set; }
        public Progress ProgressDetails { get; set; }
        public Mark MarkDetails { get; set; }
    }
}
