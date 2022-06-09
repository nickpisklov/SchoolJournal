using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolJournal.Models;
using SchoolJournal.ViewModels;

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
            //--------------------------------
            List<Lesson> lessons = _db.Lessons.Where(l => l.FkClass == classId && l.FkSchoolYear == schoolYearId
                && l.FkSubject == subjectId && l.FkTeacher == teacherId).ToList();
            List<Student> students = _db.Students.Where(s => s.FkClass == classId).OrderBy(s => s.Surname).ToList();
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
            ViewBag.SubjectTitle = _db.Subjects.Where(s => s.Id == subjectId)
                .Select(s => s.Title).FirstOrDefault().ToString();
            ViewBag.ClassTitle = _db.Classes.Where(c => c.Id == classId)
                .Select(c => c.Title).FirstOrDefault().ToString();
            return View(JournalPageList<Lesson>.Create(lessons, pageNumber ?? 1, 6));
        }
    }
}
