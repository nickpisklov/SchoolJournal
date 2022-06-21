using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using SchoolJournal.Models;
using System;
using Microsoft.AspNetCore.Http;

namespace SchoolJournal.ViewModels
{
    public enum Status
    {
        Guest,
        Student,
        Teacher,
        Admin
    }

    public class User
    {
        private Status _status = Status.Guest;
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? Class { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Middlename { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? FireDate { get; set; }

        public User() { }

        public Status GetUserStatusByLoginAndPassword(SchoolJournalContext db) 
        {
            var teacher = db.Teachers.Where(t => t.Login == Login && t.Password == Password).FirstOrDefault();
            var student = db.Students.Where(s => s.Login == Login && s.Password == Password).FirstOrDefault();
            var admin = db.Admins.Where(a => a.Login == Login && a.Password == Password).FirstOrDefault();
            if (admin == null && teacher == null && student != null)
            {
                return Status.Student;
            }
            else if (admin == null && teacher != null && student == null)
            {
                return Status.Teacher;
            }
            else if (admin != null && teacher == null && student == null)
            {
                return Status.Admin;
            }
            else 
            {
                return Status.Guest;
            }
        }
        public void SetStudentPropertiesFromDB(Student student)
        {
            Id = Convert.ToInt32(student.Id);
            Name = Convert.ToString(student.Name);
            Surname = Convert.ToString(student.Surname);
            Middlename = Convert.ToString(student.Middlename);
            Class = Convert.ToInt32(student.FkClass);
            SetStatus(Status.Student);
        }
        public void SetTeacherPropertiesFromDB(Teacher teacher)
        {
            Id = Convert.ToInt32(teacher.Id);
            Name = Convert.ToString(teacher.Name);
            Surname = Convert.ToString(teacher.Surname);
            Middlename = Convert.ToString(teacher.Middlename);
            HireDate = Convert.ToDateTime(teacher.HireDate);
            FireDate = Convert.ToDateTime(teacher.FireDate);
            SetStatus(Status.Teacher);
        }
        public void SetAdminPropertiesFromDB(Admin admin) 
        {
            Id = Convert.ToInt32(admin.Id);
            Name = Convert.ToString(admin.Login);
            Surname = Convert.ToString(admin.Password);
            SetStatus(Status.Admin);
        }
        public Status GetStatus()
        {
            return _status;
        }
        public void SetStatus(Status status)
        {
            _status = status;
        }

    }
}
