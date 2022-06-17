﻿using System.ComponentModel.DataAnnotations;

namespace SchoolJournal.Models
{
    public partial class Progress
    {
        public int FkStudent { get; set; }
        public int FkLesson { get; set; }
        public int FkMark { get; set; }

        public virtual Lesson FkLessonNavigation { get; set; }
        public virtual Mark FkMarkNavigation { get; set; }
        public virtual Student FkStudentNavigation { get; set; }
    }
}
