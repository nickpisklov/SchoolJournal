﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolJournal.Models
{
    public partial class SchoolYear
    {
        public SchoolYear()
        {
            Journals = new HashSet<Journal>();
        }

        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Journal> Journals { get; set; }

        public static List<SelectListItem> GetFkSchoolYearsSelectList(SchoolJournalContext db)
        {
            var years = db.SchoolYears;
            List<SelectListItem> fkYears = new List<SelectListItem>();
            fkYears.Add(new SelectListItem { Value = "0", Text = "Оберіть навчальний рік" });
            foreach (SchoolYear s in years)
            {
                fkYears.Add(new SelectListItem 
                { Value = s.Id.ToString(), Text = $"{s.StartDate.ToString("yyyy")}-{s.EndDate.ToString("yyyy")}" });
            }
            return fkYears;
        }
        public static int GetCurrentYearId(SchoolJournalContext db) 
        {
            return db.SchoolYears.Where(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now).Select(s => s.Id).FirstOrDefault();
        }
    }
}
