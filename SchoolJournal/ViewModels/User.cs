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

        public void SetStudentPropertiesFromDB(IQueryable<Student> students)
        {
            Id = Convert.ToInt32(students.FirstOrDefault().Id);
            Name = Convert.ToString(students.FirstOrDefault().Name);
            Surname = Convert.ToString(students.FirstOrDefault().Surname);
            Middlename = Convert.ToString(students.FirstOrDefault().Middlename);
            Class = Convert.ToInt32(students.FirstOrDefault().FkClass);
        }
        public void SetTeacherPropertiesFromDB(IQueryable<Teacher> teachers)
        {
            Id = Convert.ToInt32(teachers.FirstOrDefault().Id);
            Name = Convert.ToString(teachers.FirstOrDefault().Name);
            Surname = Convert.ToString(teachers.FirstOrDefault().Surname);
            Middlename = Convert.ToString(teachers.FirstOrDefault().Middlename);
            HireDate = Convert.ToDateTime(teachers.FirstOrDefault().HireDate);
            FireDate = Convert.ToDateTime(teachers.FirstOrDefault().FireDate);
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
