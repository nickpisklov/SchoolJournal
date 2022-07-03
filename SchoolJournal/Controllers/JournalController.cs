using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolJournal.Models;
using SchoolJournal.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SchoolJournal.Controllers
{
    public class JournalController : Controller
    {
        private readonly SchoolJournalContext _db;

        public JournalController(SchoolJournalContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Journal(int? pageNumber)
        {
            int teacherId = (int)HttpContext.Session.GetInt32("TeacherId");
            int classId = (int)HttpContext.Session.GetInt32("ClassId");
            int subjectId = (int)HttpContext.Session.GetInt32("SubjectId");
            int schoolYearId = (int)HttpContext.Session.GetInt32("YearId");
            List<Lesson> lessons = Lesson.GetLessonsForClass(_db, classId, schoolYearId, subjectId, teacherId).ToList();
            List<Student> students = Student.GetStudentsByClass(_db, classId).ToList();
            List<Progress> progresses = _db.Progresses.ToList();
            List<Mark> marks = _db.Marks.ToList();
            
            ViewBag.MarksForClass = from p in progresses
                                join l in lessons on p.FkLesson equals l.Id 
                                join s in students on p.FkStudent equals s.Id 
                                join m in marks on p.FkMark equals m.Id  
                                where l.FkClass == classId 
                                where l.FkSchoolYear == schoolYearId
                                where l.FkTeacher == teacherId 
                                where l.FkSubject == subjectId
                                select new JournalPageContent 
                                { StudentDetails = s, LessonDetails = l, ProgressDetails = p, MarkDetails = m };
            ViewBag.AllStudents = students;
            ViewBag.AllLessons = lessons;
            ViewBag.SubjectTitle = Subject.GetSubjectTitleById(_db, subjectId);
            ViewBag.ClassTitle = Class.GetClassTitleById(_db, classId);
            HttpContext.Session.SetInt32("pageNumber", pageNumber ?? 1);
            return View(JournalPaging<Lesson>.Create(lessons, pageNumber ?? 1, 15));
        }

        [HttpGet]
        public IActionResult EditLessonInfo(int lessonId) 
        {
            Lesson lesson = Lesson.GetLessonById(_db, lessonId);
            return View(lesson);
        }
        [HttpPost]
        public IActionResult EditLessonInfo(Lesson lesson)
        {
            _db.Entry(lesson).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToRoute(new { action = "Journal", controller = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") }); ;
        }

        [HttpGet]
        public IActionResult EditMark(string lessonId, string studentId, string markId) 
        {
            List<Mark> marks = _db.Marks.ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Text = "Оберіть оцінку", Value = "0" });
            foreach(Mark m in marks) 
            {
                selectListItems.Add(new SelectListItem { Value = m.Id.ToString(), Text = m.MarkValue});
            }
            Progress progress = Progress.GetProgressByStudentAndLessonId(_db, Convert.ToInt32(studentId), Convert.ToInt32(lessonId));
            if (progress == null) 
            {
                progress = new Progress(Convert.ToInt32(lessonId), Convert.ToInt32(studentId));
            }
            ViewBag.Marks = selectListItems;
            ViewBag.MarkId = Convert.ToInt32(markId);
            ViewBag.Student = Student.GetStudentById(_db, Convert.ToInt32(studentId));
            ViewBag.Lesson = Lesson.GetLessonById(_db, Convert.ToInt32(lessonId));
            return View(progress);
        }
        [HttpPost]
        public IActionResult EditMark(Progress progress) 
        {
            var progressCheck = Progress.GetProgressByStudentAndLessonId(_db, progress.FkStudent, progress.FkLesson);
            if (progress.FkMark.ToString() == "0")
            {
                return RedirectToRoute(new { action = "Journal", controller = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }
            else if (progressCheck == null) 
            {
                progress.AddToDbWithDependencies(_db, progress);
                return RedirectToRoute(new { action = "Journal", controller = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });        
            }
            else
            {
                progress.ModifyDbRecord(_db, progress, progressCheck);
                return RedirectToRoute(new { action = "Journal", controller = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }   
        }

        [HttpPost]
        public IActionResult DeleteMark(Progress progress) 
        {
            var newProgress = Progress.GetProgressByStudentAndLessonId(_db, progress.FkStudent, progress.FkLesson);
            if (newProgress == null)
            {
                return RedirectToRoute(new { action = "Journal", controller = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }
            else 
            {
                _db.Progresses.Remove(newProgress);
                _db.SaveChanges();
                return RedirectToRoute(new { action = "Journal", controller = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }
        }
    }
}
