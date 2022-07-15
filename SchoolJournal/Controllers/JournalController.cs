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
using System.Net;
using System.Net.Mail;

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
            return View(Paging<Lesson>.Create(lessons, pageNumber ?? 1, 15));
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
            Student stud=  _db.Students.Where(s => s.Id == progress.FkStudent).FirstOrDefault();
            Mark mrkname= _db.Marks.Where(s => s.Id == progress.FkMark).FirstOrDefault();
            Lesson lessonnow = _db.Lessons.Where(s => s.Id == progress.FkLesson).FirstOrDefault();
            Subject subj= _db.Subjects.Where(s => s.Id == lessonnow.FkSubject).FirstOrDefault();
            Teacher teach= _db.Teachers.Where(s => s.Id == lessonnow.FkTeacher).FirstOrDefault();
            var progressCheck = Progress.GetProgressByStudentAndLessonId(_db, progress.FkStudent, progress.FkLesson);
            if (progress.FkMark.ToString() == "0")
            {
                return RedirectToRoute(new { action = "Journal", controller = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }
            else if (progressCheck == null)
            {
                if (mrkname.MarkValue == "Н")
                {
                    sendEmail("Неявка на заннятя", "Шановні батьки! Ваша дитина " + stud.Surname + " " + stud.Name + " був(ла) відсутня на занятті " + subj.Title + " " + lessonnow.LessonDate.ToString("dd.MM.yyyy") + ". Велике прохання звернути увагу! "
                    + teach.Surname + " " + teach.Name + " " + teach.Middlename, stud.ParrentMail);
                }
                else if (mrkname.MarkValue == "1" || mrkname.MarkValue == "2" || mrkname.MarkValue == "3")
                {
                    sendEmail("Початковий бал", "Шановні батьки!\n \nВаша дитина, " + stud.Surname + " " + stud.Name + ", отримав(ла) бали початкового рівня - "
                        + mrkname.MarkValue.ToString() + ". На занятті " + subj.Title + " " + lessonnow.LessonDate.ToString("dd.MM.yyyy") +
                        ". Велике прохання звернути увагу!\n " + teach.Surname + " " + teach.Name + " " + teach.Middlename, stud.ParrentMail);
                }
                progress.AddToDbWithDependencies(_db, progress);
                return RedirectToRoute(new { action = "Journal", controller = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });        
            }
            else
            {
                if (mrkname.MarkValue == "Н")
                {
                    sendEmail("Неявка на заннятя", "Шановні батьки! Ваша дитина " + stud.Surname + " " + stud.Name + " був(ла) відсутня на занятті " + subj.Title + " " + lessonnow.LessonDate.ToString("dd.MM.yyyy") + ". Велике прохання звернути увагу! "
                    + teach.Surname + " " + teach.Name + " " + teach.Middlename, stud.ParrentMail);
                }
                else if (mrkname.MarkValue == "1" || mrkname.MarkValue == "2" || mrkname.MarkValue == "3")
                {
                    sendEmail("Початковий бал", "Шановні батьки!\n \nВаша дитина, " + stud.Surname + " " + stud.Name + ", отримав(ла) бали початкового рівня - "
                        + mrkname.MarkValue.ToString() + ". На занятті " + subj.Title + " " + lessonnow.LessonDate.ToString("dd.MM.yyyy") +
                        ". Велике прохання звернути увагу!\n " + teach.Surname + " " + teach.Name + " " + teach.Middlename, stud.ParrentMail);
                }
                progress.ModifyDbRecord(_db, progress, progressCheck);
                return RedirectToRoute(new { action = "Journal", controller = "Journal", pageNumber = HttpContext.Session.GetInt32("pageNumber") });
            }   
        }

        void sendEmail(string mainTheme, string mark, string parmail)
        {
           
            string to = parmail; //To address    
            string from = "SchoolJournal@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            string mailbody = mark;
            message.Subject = mainTheme;
            message.Body = mailbody;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("sendaspnet@gmail.com", "gvjdqdtbavbjdtxh");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
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
