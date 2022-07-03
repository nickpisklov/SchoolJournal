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

        public AdministrationController(SchoolJournalContext db)
        {
            _db = db;
        }

        public IActionResult HomeAdministration()
        {
            ViewBag.Status = Status.Admin;
            return View();
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            ViewBag.Status = Status.Admin;
            var fkClasses = Class.GetFkClassSelectList(_db);
            ViewBag.ClassesSelect = fkClasses;
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student newStudent)
        {
            var fkClasses = Class.GetFkClassSelectList(_db);
            ViewBag.ClassesSelect = fkClasses;
            if (newStudent.FkClass.ToString() == "0")
            {               
                ViewBag.Message = "Невірно обраний клас!";
                return View();
            }
            else if (newStudent.IsLoginEcxist(_db))
            {
                ViewBag.Message = "Такий логін вже існує!";
                return View();
            }
            else
            {
                newStudent.AddStudentWithDependencies(_db);
                ViewBag.Message = "Ви вдало додали учня!";
                return View();
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
            if (newClass.IsClassExists(_db))
            {
                ViewBag.Message = "Такий клас вже існує!";
                return View();
            }
            else if (newClass.Title.Length <= 4)
            {
                ViewBag.Message = "Ви вдало додали клас!";
                _db.Classes.Add(newClass);
                _db.SaveChanges();
                return View();
            }
            else
            {
                ViewBag.Message = "Ви невірно ввели назву класу! Як приклад назва має бути наприклад така - 12-Б";
                return View();
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
            if (newTeacher.HireDate > DateTime.Now)
            {
                ViewBag.Message = "Невірна дата прийому на роботу! Дата повинна бути менша за теперешню.";
                return View();
            }
            else if (newTeacher.IsLoginEcxist(_db))
            {
                ViewBag.Message = "Такий логін вже існує!";
                return View();
            }
            else
            {
                _db.Teachers.Add(newTeacher);
                _db.SaveChanges();
                ViewBag.Message = "Ви вдало додали вчителя!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddJournal()
        {
            ViewBag.FK_Classes = Class.GetFkClassSelectList(_db);
            ViewBag.FK_SchoolYears = SchoolYear.GetFkSchoolYearsSelectList(_db);
            ViewBag.FK_Teachers = Teacher.GetFkTeachersSelectList(_db);
            ViewBag.FK_Subjects = Subject.GetFkSubjectsSelectList(_db);
            return View();
        }
        [HttpPost]
        public IActionResult AddJournal(Journal newJournal) 
        {
            ViewBag.FK_Classes = Class.GetFkClassSelectList(_db);
            ViewBag.FK_SchoolYears = SchoolYear.GetFkSchoolYearsSelectList(_db);
            ViewBag.FK_Teachers = Teacher.GetFkTeachersSelectList(_db);
            ViewBag.FK_Subjects = Subject.GetFkSubjectsSelectList(_db);
            if (newJournal.IsJournalExists(_db))
            {
                ViewBag.Message = "Такий журнал вже існує!";
            }
            else 
            {
                newJournal.AddJournalWithDependencies(_db);
                ViewBag.Message = "Ви вдало додали журнал!";
            }        
            return View();
        }
    }
}
