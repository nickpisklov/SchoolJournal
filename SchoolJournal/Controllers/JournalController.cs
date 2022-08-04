using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SchoolJournal.Models;
using SchoolJournal.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace SchoolJournal.Controllers
{
    public class JournalController : Controller
    {
        private readonly SchoolJournalContext _db;
        private int _teacherId;
        private int _classId;
        private int _subjectId;
        private int _schoolYearId;
        private int _lastPage;

        public JournalController(SchoolJournalContext db)
        {
            _db = db;
        }

        public IActionResult Journal(int? pageNumber)
        {
            SetJournalFkFromSession();
            List<Lesson> lessons = _db.Lessons.Where(l => l.FkClass == _classId && l.FkSchoolYear == _schoolYearId
                && l.FkSubject == _subjectId && l.FkTeacher == _teacherId).OrderBy(l => l.LessonDate).ToList();
            List<Student> students = _db.Students.Where(s => s.FkClass == _classId).OrderBy(s => s.Surname).ToList();
            SetLastPage(lessons);
            SetViewBagVariablesForJournal(lessons, students);
            HttpContext.Session.SetInt32("pageNumber", pageNumber ?? _lastPage);
            return View(Paging<Lesson>.Create(lessons, pageNumber ?? _lastPage, 15));
        }

        [HttpGet]
        public IActionResult EditLessonInfo(int lessonId) 
        {
            Lesson lesson = _db.Lessons.Where(l => l.Id == lessonId).FirstOrDefault();
            return View(lesson);
        }
        [HttpPost]
        public IActionResult EditLessonInfo(Lesson lesson)
        {
            if (lesson.Theme == null) 
            {
                lesson.Theme = " ";
            }
            if (lesson.Homework == null) 
            {
                lesson.Homework = " ";
            }
            _db.Entry(lesson).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToRoute(new { action = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
        }
        [HttpGet]
        public IActionResult EditMark(string lessonId, string studentId, string markId) 
        {
            List<Mark> marks = _db.Marks.ToList();
            List<SelectListItem> marksSelectedList = GetMarksSelectedList(marks);
            Progress progress = _db.Progresses.Where(p => p.FkLesson == Convert.ToInt32(lessonId) 
                && p.FkStudent == Convert.ToInt32(studentId)).FirstOrDefault();
            if (progress == null) 
            {
                progress = new Progress(Convert.ToInt32(lessonId), Convert.ToInt32(studentId));
            }
            SetViewBagForEditMark(marksSelectedList, markId, studentId, lessonId);
            return View(progress);
        }
        [HttpPost]
        public IActionResult EditMark(Progress progress) 
        {
            var progressCheck = _db.Progresses.Where(p => p.FkLesson == progress.FkLesson && p.FkStudent == progress.FkStudent).FirstOrDefault();
            if (progress.FkMark.ToString() == "0")
            {
                return RedirectToRoute(new { action = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }
            else if (progressCheck == null)
            {
                progress.AddToDbWithDependencies(_db, progress);
                return RedirectToRoute(new { action = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }
            else
            {
                progress.ReplaceDbRecord(_db, progress, progressCheck);
                return RedirectToRoute(new { action = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }   
        }
        [HttpPost]
        public IActionResult DeleteMark(Progress progress)
        {
            var newProgress = _db.Progresses.Where(p => p.FkLesson == progress.FkLesson && p.FkStudent == progress.FkStudent).FirstOrDefault();
            if (newProgress == null)
            {
                return RedirectToRoute(new { action = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }
            else
            {
                _db.Progresses.Remove(newProgress);
                _db.SaveChanges();
                return RedirectToRoute(new { action = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }
        }


        private IEnumerable<JournalPageContent> GetJournalPageContent(List<Lesson> lessons, List<Student> students) 
        {
            List<Progress> progresses = _db.Progresses.ToList();
            List<Mark> marks = _db.Marks.ToList();
            return from p in progresses
                   join l in lessons on p.FkLesson equals l.Id
                   join s in students on p.FkStudent equals s.Id
                   join m in marks on p.FkMark equals m.Id
                   where l.FkClass == _classId
                   where l.FkSchoolYear == _schoolYearId
                   where l.FkTeacher == _teacherId
                   where l.FkSubject == _subjectId
                   select new JournalPageContent
                   { StudentDetails = s, LessonDetails = l, ProgressDetails = p, MarkDetails = m };
        }
       
        private List<SelectListItem> GetMarksSelectedList(List<Mark> marks) 
        {
            List<SelectListItem> marksSelectedList = new List<SelectListItem>();
            marksSelectedList.Add(new SelectListItem { Text = "Оберіть оцінку", Value = "0" });
            foreach (Mark m in marks)
            {
                marksSelectedList.Add(new SelectListItem { Value = m.Id.ToString(), Text = m.MarkValue });
            }
            return marksSelectedList;
        }
        private void SetJournalFkFromSession()
        {
            _teacherId = (int)HttpContext.Session.GetInt32("TeacherId");
            _classId = (int)HttpContext.Session.GetInt32("ClassId");
            _subjectId = (int)HttpContext.Session.GetInt32("SubjectId");
            _schoolYearId = (int)HttpContext.Session.GetInt32("YearId");
        }
        private void SetViewBagForEditMark(List<SelectListItem> marksSelectList, string markId, string studentId, string lessonId)
        {
            ViewBag.Marks = marksSelectList;
            ViewBag.MarkId = Convert.ToInt32(markId);
            ViewBag.Student = _db.Students.Where(s => s.Id == Convert.ToInt32(studentId)).FirstOrDefault();
            ViewBag.Lesson = _db.Lessons.Where(l => l.Id == Convert.ToInt32(lessonId)).FirstOrDefault();
        }
        private void SetViewBagVariablesForJournal(List<Lesson> lessons, List<Student> students)
        {
            ViewBag.MarksForClass = GetJournalPageContent(lessons, students);
            ViewBag.AllStudents = students;
            ViewBag.AllLessons = lessons;
            ViewBag.SubjectTitle = _db.Subjects.Where(s => s.Id == _subjectId).Select(s => s.Title).FirstOrDefault().ToString();
            ViewBag.ClassTitle = _db.Classes.Where(c => c.Id == _classId).Select(c => c.Title).FirstOrDefault().ToString();
        }
        private void SetLastPage(List<Lesson> lessons)
        {
            double lessonsCount = lessons.Count();
            double result = lessonsCount / 15;
            if (result <= 1)
            {
                return;
            }
            else
            {
                _lastPage = (int)Math.Truncate(result) + 1;
            }
        }
    }
}
