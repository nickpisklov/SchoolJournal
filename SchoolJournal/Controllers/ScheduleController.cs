using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private List<DateTime> CreateScheduleDays()
        {
            List<DateTime> days = new List<DateTime>();
            DateTime startDate = _db.SchoolYears.
                Where(y => y.Id == SchoolYear.GetCurrentYearId(_db)).Select(y => y.StartDate).FirstOrDefault().AddDays(89);
            days.Add(startDate);
            for (int i = 1; i <= 279; i++)
            {
                var date = startDate.AddDays(i);
                days.Add(date);
            }
            return days;
        }
        private IEnumerable<ScheduleContent> GetScheduleContent(List<Lesson> lessons)
        {
            var content = from l in lessons
                          join t in _db.Teachers on l.FkTeacher equals t.Id
                          join s in _db.Subjects on l.FkSubject equals s.Id
                          join c in _db.Classes on l.FkClass equals c.Id
                          select new ScheduleContent
                          { SubjectDetails = s, TeacherDetails = t, LessonDetails = l, ClassDetails = c };
            return content;
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
                return RedirectToAction("AdminSchedulesList");
            }
        }
        public IActionResult ClassSchedule(int? pageNumber)
        {
            var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserObject"));
            int classId = (int)user.Class;
            int schoolYearId = SchoolYear.GetCurrentYearId(_db);
            List<Lesson> lessons = Lesson.GetLessonsForClass(_db, classId, schoolYearId).ToList();
            List<DateTime> days = CreateScheduleDays();
            ViewBag.Lessons = GetScheduleContent(lessons);
            ViewBag.LessonTime = _db.LessonTimes.ToList();
            HttpContext.Session.SetInt32("pageNumber", pageNumber ?? 1);
            return View("Schedule", Paging<DateTime>.Create(days, pageNumber ?? 1, 7));
        }
        public IActionResult TeacherSchedule(int? pageNumber)
        {
            var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserObject"));
            List<Lesson> lessons = Lesson.GetLessonsForTeacher(_db, user.Id);
            int schoolYearId = SchoolYear.GetCurrentYearId(_db);
            List<DateTime> days = CreateScheduleDays();
            ViewBag.Lessons = GetScheduleContent(lessons);
            ViewBag.LessonTime = _db.LessonTimes.ToList();
            HttpContext.Session.SetInt32("pageNumber", pageNumber ?? 1);
            return View("Schedule", Paging<DateTime>.Create(days, pageNumber ?? 1, 7));
        }
        public IActionResult AdminSchedule(int? pageNumber)
        {
            int classId = Convert.ToInt32(HttpContext.Session.GetString("ClassId"));
            int schoolYearId = SchoolYear.GetCurrentYearId(_db);
            List<Lesson> lessons = Lesson.GetLessonsForClass(_db, classId, schoolYearId).ToList();
            List<DateTime> days = CreateScheduleDays();
            ViewBag.Lessons = GetScheduleContent(lessons);
            ViewBag.LessonTime = _db.LessonTimes.ToList();
            HttpContext.Session.SetInt32("pageNumber", pageNumber ?? 1);
            return View("Schedule", Paging<DateTime>.Create(days, pageNumber ?? 1, 7));
        }
        public IActionResult AdminSchedulesList()
        {
            List<Class> classes = _db.Classes.OrderBy(c => c.Title).ToList();
            return View(classes);
        }
        public IActionResult AdminSetSessionClassId(string classId)
        {
            HttpContext.Session.SetString("ClassId", classId);
            return RedirectToAction("AdminSchedule");
        }
        [HttpGet]
        public IActionResult AddLesson(string lessonDate, string fkTime)
        {
            ViewBag.Teachers = Teacher.GetFkTeachersSelectList(_db);
            ViewBag.Subjects = Subject.GetFkSubjectsSelectList(_db);
            HttpContext.Session.SetString("NewLessonsDate", lessonDate);
            HttpContext.Session.SetString("NewLessonTime", fkTime);
            return View();
        }
        [HttpPost]
        public IActionResult AddLesson(Lesson newLesson)
        {
            int year = SchoolYear.GetCurrentYearId(_db);
            string clas = HttpContext.Session.GetString("ClassId");
            string date = HttpContext.Session.GetString("NewLessonsDate");
            string time = HttpContext.Session.GetString("NewLessonTime");
            newLesson.FillLesssonProperties(clas, time, date, year);
            if (newLesson.FkSubject.ToString() == "0" || newLesson.FkTeacher.ToString() == "0")
            {
                ViewBag.Message = "Заповніть всі поля!";
                ViewBag.Teachers = Teacher.GetFkTeachersSelectList(_db);
                ViewBag.Subjects = Subject.GetFkSubjectsSelectList(_db);
                return View();
            }
            else if (_db.Journals.Where(j => j.FkClass == Convert.ToInt32(clas) 
            && j.FkSubject == newLesson.FkSubject && j.FkTeacher == newLesson.FkTeacher && j.FkSchoolYear == year)
                .FirstOrDefault() == null) 
            {
                ViewBag.Message = "У цього класу відсутній відповідний журнал!";
                ViewBag.Teachers = Teacher.GetFkTeachersSelectList(_db);
                ViewBag.Subjects = Subject.GetFkSubjectsSelectList(_db);
                return View();
            }
            else
            {
                newLesson.AddToDbWithDependencies(_db);
                return RedirectToRoute(new { action = "AdminSchedule", controller = "Schedule", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }       
        }
        [HttpGet]
        public IActionResult EditLesson(string lessonId) 
        {
            ViewBag.Teachers = Teacher.GetFkTeachersSelectList(_db);
            ViewBag.Subjects = Subject.GetFkSubjectsSelectList(_db);
            Lesson lesson = Lesson.GetLessonById(_db, Convert.ToInt32(lessonId));
            HttpContext.Session.SetString("oldLesson", JsonConvert.SerializeObject(lesson));
            return View(lesson);
        }
        [HttpPost]
        public IActionResult EditLesson(Lesson editedLesson)
        {
            Lesson oldLesson = JsonConvert.DeserializeObject<Lesson>(HttpContext.Session.GetString("oldLesson"));
            oldLesson.FkSubject = editedLesson.FkSubject;
            oldLesson.FkTeacher = editedLesson.FkTeacher;
            if (oldLesson.FkSubject.ToString()=="0" || oldLesson.FkTeacher.ToString()=="0")
            {
                ViewBag.Message = "Заповніть всі поля!";
                ViewBag.Teachers = Teacher.GetFkTeachersSelectList(_db);
                ViewBag.Subjects = Subject.GetFkSubjectsSelectList(_db);
                Lesson lesson = Lesson.GetLessonById(_db, Convert.ToInt32(oldLesson.Id));
                HttpContext.Session.SetString("oldLesson", JsonConvert.SerializeObject(lesson));
                return View(lesson);
            }
            else if (_db.Journals.Where(j => j.FkClass == oldLesson.FkClass && j.FkSubject == oldLesson.FkSubject 
            && j.FkTeacher == oldLesson.FkTeacher && j.FkSchoolYear == oldLesson.FkSchoolYear)
                .FirstOrDefault() == null)
            {
                ViewBag.Message = "У цього класу відсутній відповідний журнал!";
                ViewBag.Teachers = Teacher.GetFkTeachersSelectList(_db);
                ViewBag.Subjects = Subject.GetFkSubjectsSelectList(_db);
                return View();
            }
            else 
            {
                _db.Entry(oldLesson).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToRoute(new { action = "AdminSchedule", controller = "Schedule", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }
           
        }
        public IActionResult DeleteLesson() 
        {
            Lesson lesson = JsonConvert.DeserializeObject<Lesson>(HttpContext.Session.GetString("oldLesson"));
            if (_db.Progresses.Where(p => p.FkLesson == lesson.Id).FirstOrDefault() != null)
            {
                ViewBag.Message = "Вчитель вже виставив відмітки за цей урок!";
                return View();
            }
            else 
            {
                _db.Lessons.Remove(lesson);
                _db.SaveChanges();
                return RedirectToRoute(new { action = "AdminSchedule", controller = "Schedule", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }
        }
    }
}
