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

        private IEnumerable<HomeJournalsContent> GetHomePageDataForStudent(List<Journal> journals) 
        {
            return from j in journals
                   join c in _db.Classes on j.FkClass equals c.Id
                   join s in _db.Subjects on j.FkSubject equals s.Id
                   join t in _db.Teachers on j.FkTeacher equals t.Id
                   select new HomeJournalsContent
                   { SubjectDetails = s, ClassDetails = c, TeacherDetails = t };
        }
        private IEnumerable<HomeJournalsContent> GetHomePageDataForAdmin(List<Journal> journals) 
        {
            return from j in journals
                   join c in _db.Classes on j.FkClass equals c.Id
                   join s in _db.Subjects on j.FkSubject equals s.Id
                   join t in _db.Teachers on j.FkTeacher equals t.Id
                   select new HomeJournalsContent
                   { SubjectDetails = s, ClassDetails = c, TeacherDetails = t };
        }
        private IEnumerable<HomeJournalsContent> GetHomePageDataForTeacher(List<Journal> journals) 
        {
            return from j in journals
                   join c in _db.Classes on j.FkClass equals c.Id
                   join s in _db.Subjects on j.FkSubject equals s.Id
                   join t in _db.Teachers on j.FkTeacher equals t.Id
                   select new HomeJournalsContent
                   { SubjectDetails = s, ClassDetails = c, TeacherDetails = t };
        }

        public IActionResult Home()
        {
            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserObject"));
            if (HttpContext.Session.GetString("Status") == "Student")
            {
                List<Journal> journals = user.GetJournalsForStudent(_db);
                ViewBag.JournalsInfo = GetHomePageDataForStudent(journals);
                return View();
            }
            else if (HttpContext.Session.GetString("Status") == "Teacher")
            {
                List<Journal> journals = user.GetJournalsForTeacher(_db);
                ViewBag.JournalsInfo = GetHomePageDataForTeacher(journals);
                return View();
            }
            else
            {
                List<Journal> journals = user.GetJournalsForAdmin(_db);
                ViewBag.JournalsInfo = GetHomePageDataForAdmin(journals);
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
