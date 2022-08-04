using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolJournal.Models;
using SchoolJournal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace SchoolJournal.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly SchoolJournalContext _db;
        private SchoolYear _currentSchoolYear;

        public AdministrationController(SchoolJournalContext db)
        {
            _db = db;
            _currentSchoolYear = _db.SchoolYears.Where(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now).FirstOrDefault();
        }

        public IActionResult HomeAdministration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            ViewBag.ClassesSelect = GetFkClassSelectList();
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student newStudent)
        {
            ViewBag.ClassesSelect = GetFkClassSelectList();
            if (IsLoginExist(newStudent.Login))
            {
                ViewBag.IsFormValid = false;
                ViewBag.Message = "Такий логін вже існує!";
                return View();
            }
            else
            {
                if (ModelState.IsValid) 
                {
                    newStudent.AddStudentWithDependencies(_db);
                    ViewBag.IsFormValid = true;
                    ViewBag.Message = "Ви вдало додали учня!";
                    return View();
                }
                else 
                {
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult AddTeacher()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTeacher(Teacher newTeacher)
        {
            if (IsLoginExist(newTeacher.Login))
            {
                ViewBag.IsFormValid = false;
                ViewBag.Message = "Такий логін вже існує!";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _db.Teachers.Add(newTeacher);
                    _db.SaveChanges();
                    ViewBag.IsFormValid = true;
                    ViewBag.Message = "Ви вдало додали вчителя!";
                    return View();
                }
                else 
                {
                    return View();
                }
            }
        }

        [HttpGet]
        public IActionResult AddClass()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddClass(Class newClass)
        {
            if (IsClassExists(newClass.Title, newClass.RecruitmentDate))
            {
                ViewBag.IsClassValid = false;
                ViewBag.Message = "Такий клас вже існує!";
                return View();
            }
            else if (newClass.RecruitmentDate.Day != 1 || newClass.RecruitmentDate.Month != 9) 
            {
                ViewBag.IsDateValid = false;
                ViewBag.Message = "Невірна дата набору класу. Дата набору повинна відповідати наступному шаблону 01.09.[Рік набору]";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    ViewBag.IsFormValid = true;
                    ViewBag.Message = "Ви вдало додали клас!";
                    _db.Classes.Add(newClass);
                    _db.SaveChanges();
                    return View();
                }
                else
                {
                    return View();
                }
            }       
        }

        [HttpGet]
        public IActionResult AddJournal()
        {
            SetViewBagForJournalAdding();
            return View();
        }
        [HttpPost]
        public IActionResult AddJournal(Journal newJournal) 
        {
            SetViewBagForJournalAdding();
            newJournal.FkSchoolYear = SchoolYear.GetCurrentYearId(_db);
            if (IsJournalExists(newJournal))
            {
                ViewBag.IsClassValid = false;
                ViewBag.Message = "Такий журнал вже існує!";
                return View();
            }
            else 
            {
                if (ModelState.IsValid)
                {
                    ViewBag.IsClassValid = true;
                    newJournal.AddJournalWithDependencies(_db);
                    ViewBag.Message = "Ви вдало додали журнал!";
                    return View();
                }
                else
                {
                    return View();
                }
            }
        }

        private List<SelectListItem> GetFkSubjectsSelectList()
        {
            var subjects = _db.Subjects;
            List<SelectListItem> fkSubjects = new List<SelectListItem>();
            fkSubjects.Add(new SelectListItem { Value = "0", Text = "Оберіть предмет" });
            foreach (Subject s in subjects)
            {
                fkSubjects.Add(new SelectListItem
                { Value = s.Id.ToString(), Text = s.Title });
            }
            return fkSubjects;
        }
        private List<SelectListItem> GetFkTeachersSelectList()
        {
            var teachers = _db.Teachers.Where(t => t.FireDate == null);
            List<SelectListItem> fkTeachers = new List<SelectListItem>();
            fkTeachers.Add(new SelectListItem { Value = "0", Text = "Оберіть вчителя" });
            foreach (Teacher t in teachers)
            {
                fkTeachers.Add(new SelectListItem
                { Value = t.Id.ToString(), Text = $"{t.Surname} {t.Name} {t.Middlename}" });
            }
            return fkTeachers;
        }
        private List<SelectListItem> GetFkClassSelectList()
        {
            var classes = _db.Classes.OrderBy(c => c.Title.Length).ThenBy(c => c.Title);
            List<SelectListItem> fkClasses = new List<SelectListItem>();
            fkClasses.Add(new SelectListItem { Value = "0", Text = "Оберіть клас" });
            foreach (Class c in classes)
            {
                fkClasses.Add(new SelectListItem { Value = c.Id.ToString(), Text = c.Title });
            }
            return fkClasses;
        }
        private bool IsLoginExist(string login) 
        {
            var t = _db.Teachers.Where(t => t.Login == login).FirstOrDefault();
            var s = _db.Students.Where(s => s.Login == login).FirstOrDefault();
            var a = _db.Admins.Where(a => a.Login == login).FirstOrDefault();
            if (t == null && s == null && a == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool IsJournalExists(Journal journal)
        {
            var journalCheck = _db.Journals.Where(j => j.FkClass == journal.FkClass && j.FkSchoolYear == journal.FkSchoolYear
            && j.FkSubject == journal.FkSubject && j.FkTeacher == journal.FkTeacher).FirstOrDefault();
            if (journal == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool IsClassExists(string title, DateTime recruitmentDate)
        {
            var classCheck = _db.Classes.Where(c => c.Title == title && c.RecruitmentDate.Equals(recruitmentDate)).FirstOrDefault();
            if (classCheck == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void SetViewBagForJournalAdding() 
        {
            ViewBag.FK_Classes = GetFkClassSelectList();
            ViewBag.FK_Teachers = GetFkTeachersSelectList();
            ViewBag.FK_Subjects = GetFkSubjectsSelectList();
        }
        public void AddJournalWithDependencies(SchoolJournalContext db)
        {
            var classDependencies = db.Classes.Find(FkClass);
            classDependencies.Journals.Add(this);
            var subjectDependencies = db.Subjects.Find(FkSubject);
            subjectDependencies.Journals.Add(this);
            var yearDependencies = db.SchoolYears.Find(FkSchoolYear);
            yearDependencies.Journals.Add(this);
            var teacherDependencies = db.Teachers.Find(FkTeacher);
            teacherDependencies.Journals.Add(this);
            db.Journals.Add(this);
            db.SaveChanges();
        }
    }
}
