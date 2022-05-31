using System;
using System.Collections.Generic;

namespace SchoolJournal.Data.Models
namespace SchoolJournal
{
    public partial class Lesson
    {
        public Lesson()
        {
            Progresses = new HashSet<Progress>();
        }

        public int Id { get; set; }
        public int FkTeacher { get; set; }
        public int FkSubject { get; set; }
        public int FkClass { get; set; }
        public int FkSchoolYear { get; set; }
        public int FkLessonTime { get; set; }
        public DateTime LessonDate { get; set; }
        public string Theme { get; set; }
        public string Homework { get; set; }

        public virtual Journal Fk { get; set; }
        public virtual LessonTime FkLessonTimeNavigation { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; }
    }
}
