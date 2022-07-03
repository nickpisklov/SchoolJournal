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
            HttpContext.Session.SetString("Status", "Guest");
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(User user)
        {
            if (user.GetUserStatusByLoginAndPassword(_db) == Status.Teacher)
            {
                user.SetTeacherPropertiesFromDB(Teacher.GetTeacherByLogin(_db, user.Login));
                HttpContext.Session.SetString("UserObject", JsonConvert.SerializeObject(user));
                HttpContext.Session.SetString("Status", "Teacher");
                return RedirectToAction("Home", "Home");
            }
            else if (user.GetUserStatusByLoginAndPassword(_db) == Status.Student)
            {
                user.SetStudentPropertiesFromDB(Student.GetStudentByLogin(_db, user.Login));
                HttpContext.Session.SetString("UserObject", JsonConvert.SerializeObject(user));
                HttpContext.Session.SetString("Status", "Student");
                return RedirectToAction("Home", "Home");
            }
            else if (user.GetUserStatusByLoginAndPassword(_db) == Status.Admin) 
            {
                user.SetAdminPropertiesFromDB(Admin.GetAdminByLogin(_db, user.Login));
                HttpContext.Session.SetString("UserObject", JsonConvert.SerializeObject(user));
                HttpContext.Session.SetString("Status", "Admin");
                return RedirectToAction("HomeAdministration", "Administration");
            }
            else
            {
                ViewBag.IsGuest = true;
                return View();
            }                    
        }
    }
}
