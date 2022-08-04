using Microsoft.AspNetCore.Mvc;
using SchoolJournal.Models;
using Microsoft.AspNetCore.Http;
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
        private void SetSessionVariablesForAdmin(User user) 
        {
            HttpContext.Session.SetString("UserObject", JsonConvert.SerializeObject(user));
            HttpContext.Session.SetString("Status", "Admin");
        }
        private void SetSessionVariablesForStudent(User user) 
        {
            HttpContext.Session.SetString("UserObject", JsonConvert.SerializeObject(user));
            HttpContext.Session.SetString("Status", "Student");
        }
        private void SetSessionVariablesForTeacher(User user) 
        {
            HttpContext.Session.SetString("UserObject", JsonConvert.SerializeObject(user));
            HttpContext.Session.SetString("Status", "Teacher");
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
                SetSessionVariablesForTeacher(user);
                return RedirectToAction("Home", "Home");
            }
            else if (user.GetUserStatusByLoginAndPassword(_db) == Status.Student)
            {
                user.SetStudentPropertiesFromDB(Student.GetStudentByLogin(_db, user.Login));
                SetSessionVariablesForStudent(user);
                return RedirectToAction("Home", "Home");
            }
            else if (user.GetUserStatusByLoginAndPassword(_db) == Status.Admin) 
            {
                user.SetAdminPropertiesFromDB(Admin.GetAdminByLogin(_db, user.Login));
                SetSessionVariablesForAdmin(user);
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
