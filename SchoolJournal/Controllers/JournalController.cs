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
    }
}
