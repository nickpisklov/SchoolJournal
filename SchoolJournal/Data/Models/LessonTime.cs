using System.Collections.Generic;

namespace SchoolJournal
{
    public partial class LessonTime
    {
        public LessonTime()
        {
            Lessons = new HashSet<Lesson>();
        }

        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
