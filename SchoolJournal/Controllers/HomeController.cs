using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using SchoolJournal.ViewModels;
using System.Collections.Generic;
using SchoolJournal.Models;
using System.Linq;
using System;

namespace SchoolJournal.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolJournalContext _db;

        public HomeController(SchoolJournalContext db)
        {
            _db = db;
        }

        public IActionResult Home()
        {
            var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserObject"));
            var classes = _db.Classes;
            var subjects = _db.Subjects;
            var teachers = _db.Teachers;
            if (HttpContext.Session.GetString("Status") == "Student")
            {
                List<Journal> journals = user.GetJournalsForStudent(_db);
                ViewBag.JournalsInfo = from j in journals
                                       join c in classes on j.FkClass equals c.Id
                                       join s in subjects on j.FkSubject equals s.Id
                                       join t in teachers on j.FkTeacher equals t.Id
                                       select new HomeJournalsContent
                                       { SubjectDetails = s, ClassDetails = c, TeacherDetails = t };
                return View();
            }
            else if (HttpContext.Session.GetString("Status") == "Teacher")
            {
                List<Journal> journals = user.GetJournalsForTeacher(_db);
                ViewBag.JournalsInfo = from j in journals
                                       join c in classes on j.FkClass equals c.Id
                                       join s in subjects on j.FkSubject equals s.Id
                                       join t in teachers on j.FkTeacher equals t.Id
                                       select new HomeJournalsContent
                                       { SubjectDetails = s, ClassDetails = c, TeacherDetails = t };
                return View();
            }
            else
            {
                List<Journal> journals = user.GetJournalsForAdmin(_db);
                ViewBag.JournalsInfo = from j in journals
                                       join c in classes on j.FkClass equals c.Id
                                       join s in subjects on j.FkSubject equals s.Id
                                       join t in teachers on j.FkTeacher equals t.Id
                                       select new HomeJournalsContent
                                       { SubjectDetails = s, ClassDetails = c, TeacherDetails = t };
                return View();
            }
                     
        }

        [HttpGet]
        public IActionResult ViewJournal(string subjectId, string teacherId, string classId) 
        {
            HttpContext.Session.SetInt32("SubjectId", Convert.ToInt32(subjectId));
            HttpContext.Session.SetInt32("ClassId", Convert.ToInt32(classId));
            HttpContext.Session.SetInt32("TeacherId", Convert.ToInt32(teacherId));
            HttpContext.Session.SetInt32("YearId", SchoolYear.GetCurrentYearId(_db));
            return RedirectToAction("Journal", "Journal");
        }
    }
}
