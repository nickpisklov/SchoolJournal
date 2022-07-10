using SchoolJournal.Models;

namespace SchoolJournal.ViewModels
{
    public class ScheduleContent
    {
        public Lesson LessonDetails { get; set; }
        public Teacher TeacherDetails { get; set; }
        public Subject SubjectDetails { get; set; }
        public Class ClassDetails { get; set; }
    }
}
