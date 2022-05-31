using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SchoolJournal.Models
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
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        private Status _status = Status.Guest;   

        public User() { }

        public bool StudentStatusCheck(IConfiguration configuration) 
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM Student WHERE [Login] = '{Login}' " +
                $"AND [Password] = '{Password}';", connection);
            int countOfUsers = (int)command.ExecuteScalar();
            connection.Close();
            if (countOfUsers > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }           
        }
        public bool TeacherStatusCheck(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM Teacher WHERE [Login] = '{Login}' " +
                $"AND [Password] = '{Password}';", connection);
            int countOfUsers = (int)command.ExecuteScalar();
            connection.Close();
            if (countOfUsers > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AdminStatusCheck(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM Admin WHERE [Login] = '{Login}' " +
                $"AND [Password] = '{Password}';", connection);
            int countOfUsers = (int)command.ExecuteScalar();
            connection.Close();
            if (countOfUsers > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
