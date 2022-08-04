using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SchoolJournal.Models
{
    public partial class Journal
    {
        public Journal()
        {
            Lessons = new HashSet<Lesson>();
        }

        [RegularExpression("[^0]", ErrorMessage = "Будь ласка, оберіть вчителя!")]
        public int FkTeacher { get; set; }
        [RegularExpression("[^0]", ErrorMessage = "Будь ласка, оберіть предмет!")]
        public int FkSubject { get; set; }
        [RegularExpression("[^0]", ErrorMessage = "Будь ласка, оберіть клас!")]
        public int FkClass { get; set; }
        [RegularExpression("[^0]", ErrorMessage = "Будь ласка, оберіть навчальний рік!")]
        public int FkSchoolYear { get; set; }

        public virtual Class FkClassNavigation { get; set; }
        public virtual SchoolYear FkSchoolYearNavigation { get; set; }
        public virtual Subject FkSubjectNavigation { get; set; }
        public virtual Teacher FkTeacherNavigation { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }

        
        
    }
}
