using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SchoolJournal.Models;

namespace SchoolJournal.Controllers
{
    public class AuthorizationController : Controller
    {
        private IConfiguration configuration;

        public AuthorizationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ViewResult Index(User user)
        {
            bool isTeacher = user.TeacherStatusCheck(configuration);
            bool IsStudent = user.StudentStatusCheck(configuration);

            if (isTeacher == false && IsStudent == false)
            {
                ViewBag.IsGuest = true;
                return View("Index", user);
            }
            else if (isTeacher == true && IsStudent == false)
            {
                ViewBag.IsGuest = false;
                ViewBag.Status = "'Вчитель'";
                //Set fields by login
                return View("Index", user);
            }
            else 
            {
                ViewBag.IsGuest = false;
                ViewBag.Status = "'Учень'";
                //Set fields by login
                return View("Index", user);
            }
        }
    }
}
