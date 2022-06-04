using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SchoolJournal.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using Newtonsoft.Json;
using SchoolJournal.ViewModels;

namespace SchoolJournal.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly SchoolJournalContext _db;

        public AuthorizationController(SchoolJournalContext db) 
        {
            _db = db;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(User user)
        {
            var teachers = _db.Teachers
                .Where(t => t.Login == user.Login && t.Password == user.Password);

            var students = _db.Students
                .Where(s => s.Login == user.Login && s.Password == user.Password);

            if (teachers.FirstOrDefault() != null && students.FirstOrDefault() == null)
            {                
                ViewBag.IsGuest = false;
                user.SetTeacherPropertiesFromDB(teachers);
                user.SetStatus(Status.Teacher);
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                return RedirectToAction("Home");
            }
            else if (teachers.FirstOrDefault() == null && students.FirstOrDefault() != null)
            {
                ViewBag.IsGuest = false;
                user.SetStudentPropertiesFromDB(students);
                user.SetStatus(Status.Student);
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                return RedirectToAction("Home");
            }
            else
            {
                ViewBag.IsGuest = true;
                return View();
            }
                       
        }
        public IActionResult Home()
        {
            var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            return View();
        }
    }
}
