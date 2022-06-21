using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolJournal.Models;
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
            return View();
        }

        [HttpGet]
        public IActionResult AddStudent() 
        {
            var fkClasses = Class.GetFkClassSelectList(_db);
            ViewBag.ClassesSelect = fkClasses;
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student newStudent)
        {
            if (newStudent.FkClass.ToString() == "0")
            {
                var fkClasses = Class.GetFkClassSelectList(_db);
                ViewBag.ClassesSelect = fkClasses;
                ViewBag.Message = "Невірно обраний клас!";
                return View();
            }
            else if (newStudent.IsLoginEcxist(_db, newStudent)) 
            {
                var fkClasses = Class.GetFkClassSelectList(_db);
                ViewBag.ClassesSelect = fkClasses;
                ViewBag.Message = "Такий логін вже існує!";
                return View();
            }
            else
            {
                newStudent.AddStudentWithDependencies(_db, newStudent);
                var fkClasses = Class.GetFkClassSelectList(_db);
                ViewBag.ClassesSelect = fkClasses;
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
            if (newClass.Title.Length <= 4)
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
            else if (newTeacher.IsLoginEcxist(_db, newTeacher)) 
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
    }
}
