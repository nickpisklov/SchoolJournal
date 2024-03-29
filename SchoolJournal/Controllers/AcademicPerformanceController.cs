﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SchoolJournal.Models;
using SchoolJournal.ViewModels;

namespace SchoolJournal.Controllers
{
    public class AcademicPerformanceController : Controller
    {
        private readonly SchoolJournalContext _db;

        public AcademicPerformanceController(SchoolJournalContext db)
        {
            _db = db;
        }


        public IActionResult SubjectList() 
        {
            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserObject"));
            List<Journal> journals = _db.Journals.
                Where(j => j.FkClass == user.Class && j.FkSchoolYear==SchoolYear.GetCurrentYearId(_db)).ToList();
            var subjects = from j in journals
                           join s in _db.Subjects on j.FkSubject equals s.Id
                           select new SubjectListContent
                           { SubjectDetails = s, JournalDetails = j };
            return View(subjects);
        }
        public IActionResult AcademicPerformance(string fkSubject, string fkTeacher)
        {
            User user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserObject"));
            SetViewBagForAcademicPerformance(user, Convert.ToInt32(fkSubject), Convert.ToInt32(fkTeacher));
            return View();
        }


        private void SetViewBagForAcademicPerformance(User user, int subjectId, int teacherId) 
        {
            ViewBag.High = GetHighMarksCount(user, teacherId, subjectId);
            ViewBag.MidHigh = GetMidHighMarksCount(user, teacherId, subjectId);
            ViewBag.Mid = GetMidMarksCount(user, teacherId, subjectId);
            ViewBag.Low = GetLowMarksCount(user, teacherId, subjectId);
            ViewBag.Average = GetAverageMark(user, teacherId, subjectId);
            ViewBag.Eps = GetEpsCount(user, teacherId, subjectId);
            ViewBag.Student = user;
            ViewBag.Subject = _db.Subjects.Where(s => s.Id == subjectId).Select(s => s.Title).FirstOrDefault().ToString();
        }
        private int GetHighMarksCount(User user, int teacherId, int subjectId) 
        {
            return (from l in _db.Lessons
                    join p in _db.Progresses on l.Id equals p.FkLesson
                    where p.FkStudent == user.Id && p.FkMark >= 10 && p.FkMark <= 12
                    && l.FkClass == user.Class && l.FkTeacher == teacherId
                    && l.FkSchoolYear == SchoolYear.GetCurrentYearId(_db) && l.FkSubject == subjectId
                    select p.FkMark).Count();
        }
        private int GetMidHighMarksCount(User user, int teacherId, int subjectId) 
        {
            return (from l in _db.Lessons
                    join p in _db.Progresses on l.Id equals p.FkLesson
                    where p.FkStudent == user.Id && p.FkMark >= 7 && p.FkMark <= 9
                    && l.FkClass == user.Class && l.FkTeacher == teacherId
                    && l.FkSchoolYear == SchoolYear.GetCurrentYearId(_db) && l.FkSubject == subjectId
                    select p.FkMark).Count();
        }
        private int GetMidMarksCount(User user, int teacherId, int subjectId)
        {
            return (from l in _db.Lessons
                    join p in _db.Progresses on l.Id equals p.FkLesson
                    where p.FkStudent == user.Id && p.FkMark >= 4 && p.FkMark <= 6
                    && l.FkClass == user.Class && l.FkTeacher == teacherId
                    && l.FkSchoolYear == SchoolYear.GetCurrentYearId(_db) && l.FkSubject == subjectId
                    select p.FkMark).Count();
        }
        private int GetLowMarksCount(User user, int teacherId, int subjectId)
        {
            return (from l in _db.Lessons
                    join p in _db.Progresses on l.Id equals p.FkLesson
                    where p.FkStudent == user.Id && p.FkMark >= 1 && p.FkMark <= 3
                    && l.FkClass == user.Class && l.FkTeacher == teacherId
                    && l.FkSchoolYear == SchoolYear.GetCurrentYearId(_db) && l.FkSubject == subjectId
                    select p.FkMark).Count();
        }
        private int GetEpsCount(User user, int teacherId, int subjectId) 
        {
            return (from l in _db.Lessons
                    join p in _db.Progresses on l.Id equals p.FkLesson
                    where p.FkStudent == user.Id && p.FkMark == 13
                    && l.FkClass == user.Class && l.FkTeacher == teacherId
                    && l.FkSchoolYear == SchoolYear.GetCurrentYearId(_db) && l.FkSubject == subjectId
                    select p.FkMark).Count();
        }
        private double GetAverageMark(User user, int teacherId, int subjectId) 
        {
            double marksSum = (from l in _db.Lessons
                            join p in _db.Progresses on l.Id equals p.FkLesson
                            where p.FkStudent == user.Id && p.FkMark < 13
                            && l.FkClass == user.Class && l.FkTeacher == teacherId
                            && l.FkSchoolYear == SchoolYear.GetCurrentYearId(_db) && l.FkSubject == subjectId
                            select p.FkMark).Sum();
            double markCount = (from l in _db.Lessons
                             join p in _db.Progresses on l.Id equals p.FkLesson
                             where p.FkStudent == user.Id && p.FkMark < 13
                             && l.FkClass == user.Class && l.FkTeacher == teacherId
                             && l.FkSchoolYear == SchoolYear.GetCurrentYearId(_db) && l.FkSubject == subjectId
                             select p.FkMark).Count();
            if (markCount == 0)
            {
                return 0;
            }
            else 
            {
                return marksSum / markCount;
            }   
        }
    }
}
