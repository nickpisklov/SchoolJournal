using SchoolJournal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.ViewModels
{
    public class HomeJournalsContent
    {
        public Teacher TeacherDetails { get; set; }
        public Subject SubjectDetails { get; set; }
        public Class ClassDetails { get; set; }
    }
}
