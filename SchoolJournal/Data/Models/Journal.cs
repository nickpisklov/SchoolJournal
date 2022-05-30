using System.Collections.Generic;

namespace SchoolJournal
{
    public partial class Journal
    {
        public Journal()
        {
            Lessons = new HashSet<Lesson>();
        }

        public int FkTeacher { get; set; }
        public int FkSubject { get; set; }
        public int FkClass { get; set; }
        public int FkSchoolYear { get; set; }

        public virtual Class FkClassNavigation { get; set; }
        public virtual SchoolYear FkSchoolYearNavigation { get; set; }
        public virtual Subject FkSubjectNavigation { get; set; }
        public virtual Teacher FkTeacherNavigation { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
