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
            //-----From session variables-----
            int teacherId = 1;
            int classId = 1;
            int subjectId = 1;
            int schoolYearId = 2;
            ViewBag.Status = Status.Teacher;
            //--------------------------------
            List<Lesson> lessons = Lesson.GetLessonsForClass(_db, classId, schoolYearId, subjectId, teacherId).ToList();
            List<Student> students = Student.GetStudentsByClass(_db, classId).ToList();
            List<Progress> progresses = _db.Progresses.ToList();
            List<Mark> marks = _db.Marks.ToList();
            ViewBag.MarksForClass = from p in progresses
                                join l in lessons on p.FkLesson equals l.Id into table1
                                from l in table1.DefaultIfEmpty()
                                join s in students on p.FkStudent equals s.Id into table2
                                from s in table2.DefaultIfEmpty()
                                join m in marks on p.FkMark equals m.Id into table3
                                from m in table3.DefaultIfEmpty()
                                select new JournalPageContent 
                                { StudentDetails = s, LessonDetails = l, ProgressDetails = p, MarkDetails = m };
            ViewBag.AllStudents = students;
            ViewBag.AllLessons = lessons;
            ViewBag.SubjectTitle = Subject.GetSubjectTitleById(_db, subjectId);
            ViewBag.ClassTitle = Class.GetClassTitleById(_db, classId);
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
            return RedirectToAction("Journal");
        }


        [HttpGet]
        public IActionResult EditMark(string lessonId, string studentId, string markId) 
        {
            List<Mark> marks = _db.Marks.ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach(Mark m in marks) 
            {
                selectListItems.Add(new SelectListItem { Value = m.Id.ToString(), Text = m.MarkValue});
            }
            Progress progress = _db.Progresses.Where(p => p.FkLesson == Convert.ToInt32(lessonId) && p.FkMark == Convert.ToInt32(markId)
            && p.FkStudent == Convert.ToInt32(studentId)).FirstOrDefault();
            if (progress == null) 
            {
                progress = new Progress()
                {
                    FkLesson = Convert.ToInt32(lessonId),
                    FkStudent = Convert.ToInt32(studentId),
                    FkMark = 1
                };
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
            var toDelete = _db.Progresses.Where(p => p.FkLesson == progress.FkLesson && p.FkStudent == progress.FkStudent).FirstOrDefault();
            if (toDelete == null) 
            {
                var student = _db.Students.Find(progress.FkStudent);
                student.Progresses.Add(progress);
                var lesson = _db.Lessons.Find(progress.FkLesson);
                lesson.Progresses.Add(progress);
                _db.Progresses.Add(progress);
                _db.SaveChanges();
                return RedirectToAction("Journal");
            }
            else 
            {
                _db.Progresses.Remove(toDelete);
                _db.SaveChanges();
                _db.Entry(progress).State = EntityState.Added;
                _db.SaveChanges();
                return RedirectToAction("Journal");
            }   
        }
    }
}
