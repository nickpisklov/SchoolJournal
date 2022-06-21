using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SchoolJournal.Models
{
    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Login { get; set; }
        [Required]
        [StringLength(150)]
        public string Password { get; set; }

        public static Admin GetAdminByLogin(SchoolJournalContext db, string login)
        {
            return db.Admins.Where(s => s.Login == login).FirstOrDefault();
        }
    }
}
