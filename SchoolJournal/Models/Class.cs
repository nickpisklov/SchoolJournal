using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SchoolJournal.Models
{
    public partial class Class
    {
        public Class()
        {
            Journals = new HashSet<Journal>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        [RegularExpression("^([1-9]|1[01])[-][А-Я]{1}$", ErrorMessage = "Неправильна назва класу! Шаблон назви класу [Число від 1 до 11]-[Велика літера]")]
        [Required(ErrorMessage = "Будь ласка, введіть назву класу!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Будь ласка, введіть дату набору!")]
        public DateTime RecruitmentDate { get; set; }

        public virtual ICollection<Journal> Journals { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        
    }
}
