using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SchoolJournal.Models
{
    public partial class Student
    {
        public Student()
        {
            Progresses = new HashSet<Progress>();
        }

        public int Id { get; set; }
        [RegularExpression("[^0]", ErrorMessage = "Будь ласка, оберіть клас!")]
        public int FkClass { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть логін учня!")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть пароль учня!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть ім'я учня!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть прізвище учня!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть по-батькові учня!")]
        public string Middlename { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть email адресу одного з батьків!")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email адреса введена неправильно!")]
        public string ParrentMail { get; set; }

        public virtual Class FkClassNavigation { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; }

        public static IQueryable<Student>GetStudentsByClass(SchoolJournalContext db, int classId) 
        {
            return db.Students.Where(s => s.FkClass == classId).OrderBy(s => s.Surname);
        }
        public void AddStudentWithDependencies(SchoolJournalContext db) 
        {
            var classDependencies = db.Classes.Find(FkClass);
            classDependencies.Students.Add(this);
            db.Students.Add(this);
            db.SaveChanges();
        }

        public static Student GetStudentById(SchoolJournalContext db, int id)
        {
            return db.Students.Where(s => s.Id == id).FirstOrDefault();
        }
        public static Student GetStudentByLogin(SchoolJournalContext db, string login)
        {
            return db.Students.Where(s => s.Login == login).FirstOrDefault();
        }
    }
}
