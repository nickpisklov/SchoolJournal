using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolJournal.Models;
using SchoolJournal.ViewModels;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly SchoolJournalContext _db;

        public ScheduleController(SchoolJournalContext db)
        {
            _db = db;
        }
        public IActionResult ScheduleRedirect() 
        {
            if (HttpContext.Session.GetString("Status") == "Teacher")
            {
                return RedirectToAction("TeacherSchedule");
            }
            else if (HttpContext.Session.GetString("Status") == "Student")
            {
                return RedirectToAction("ClassSchedule");
            }
            else 
            {
                return RedirectToAction("AdminSchedule");
            }
        }
        public IActionResult ClassSchedule(int? pageNumber)
        {
            var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserObject"));
            int classId = (int)user.Class;
            int schoolYearId = SchoolYear.GetCurrentYearId(_db);
            List<DateTime> days = new List<DateTime>();
            DateTime startDate = _db.SchoolYears.Where(y => y.Id == schoolYearId).Select(y => y.StartDate).FirstOrDefault().AddDays(89);
            days.Add(startDate);
            for (int i = 1; i <= 280; i++) 
            {
                var date = startDate.AddDays(i);
                days.Add(date);
            }
            ViewBag.Lessons = Lesson.GetLessonsForClass(_db, classId, schoolYearId).ToList();
            ViewBag.LessonTime = _db.LessonTimes.ToList();
            HttpContext.Session.SetInt32("pageNumber", pageNumber ?? 1);
            return View(Paging<DateTime>.Create(days, pageNumber ?? 1, 7));
        }
        public IActionResult TeacherSchedule() 
        {
            return View();
        }
        public IActionResult AdminSchedule() 
        {
            return View();
        }
    }
}
