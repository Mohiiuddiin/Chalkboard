using ChalkboardAPI.Helpers;
using ChalkboardAPI.Models;
using ESCHOOL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ESCHOOL.Services
{
    public interface ITasksServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Tasks> GetAll();
        Tasks GetById(int id);
        //IEnumerable<Tasks> GetTaskListBySubjectId(int id);
        //IEnumerable<Subjects> GetSubjectsByStudentId(string sid);
    }
    public class TasksServices: ITasksServices
    {
        private List<Tasks> _students = new List<Tasks>
        {
            new Tasks { TaskId = 1,SectionId=1, TaskDate = DateTime.Now,
                TaskDetails="test", EntryBy = "User", EntryDate = DateTime.Now
                 }
        };
        private readonly IConfiguration _configuration;

        private readonly AppSettings _appSettings;

        public TasksServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _configuration = configuration;

            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.TaskDetails == model.Email && x.TaskDetails == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<Tasks> GetAll()
        {

            List<Tasks> studentProfileViews = new List<Tasks>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Tasks";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Tasks studentProfileView = new Tasks();
                studentProfileView.TaskId = Convert.ToInt32(reader["TaskId"]);
                studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
                studentProfileView.TaskDate = Convert.ToDateTime(reader["TaskDate"]);
                //studentProfileView.TaskHeadline = reader["TaskHeadline"].ToString();
                studentProfileView.TaskDetails = reader["TaskDetails"].ToString();
                studentProfileView.EntryBy = reader["EntryBy"].ToString();

                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);

               

                //studentProfileView.Name = reader["Name"].ToString();
                

                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;

            // return _students;
        }

        //public IEnumerable<Tasks> GetTaskListBySubjectId(int id)
        //{

        //    List<Tasks> studentProfileViews = new List<Tasks>();

        //    string connectionString = _configuration.GetConnectionString("StudentDB");
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    string query = "Select * from Tasks where SubjectId=" +id;
        //    SqlCommand com = new SqlCommand(query, connection);
        //    connection.Open();
        //    SqlDataReader reader = com.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        Tasks studentProfileView = new Tasks();
        //        studentProfileView.TaskId = Convert.ToInt32(reader["TaskId"]);
        //        studentProfileView.SubjectId = Convert.ToInt32(reader["SubjectId"]);
        //        studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
        //        studentProfileView.TaskDate = Convert.ToDateTime(reader["TaskDate"]);
        //        //studentProfileView.TaskHeadline = reader["TaskHeadline"].ToString();
        //        studentProfileView.TaskDetails = reader["TaskDetails"].ToString();
        //        studentProfileView.EntryBy = reader["EntryBy"].ToString();

        //        studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);



        //        //studentProfileView.Name = reader["Name"].ToString();


        //        studentProfileViews.Add(studentProfileView);
        //    }
        //    reader.Close();
        //    connection.Close();
        //    return studentProfileViews;

        //    // return _students;
        //}

        public Tasks GetById(int id)
        {
            Tasks studentProfileView = new Tasks();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Tasks where TaskId=" + id + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.TaskId = Convert.ToInt32(reader["TaskId"]);
                studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                studentProfileView.TaskDate = Convert.ToDateTime(reader["TaskDate"]);



                //studentProfileView.Name = reader["Name"].ToString();
                //studentProfileView.TaskHeadline = reader["TaskHeadline"].ToString();
                studentProfileView.TaskDetails = reader["TaskDetails"].ToString();
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
            }
            return studentProfileView;
            // return _students.FirstOrDefault(x => x.TaskId == id);
        }

        // helper methods

        //public IEnumerable<Subjects> GetSubjectsByStudentId(string id)
        //{
        //    List<Subjects> studentProfileViews = new List<Subjects>();

        //    string connectionString = _configuration.GetConnectionString("StudentDB");
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    string query = "Select s.SubjecId,s.SubjectName from Subjects s left join Classes c on c.ClassId = s.ClassId left join Students std on std.ClassId = c.ClassId where std.StudentID='" + id + "'";
        //    SqlCommand com = new SqlCommand(query, connection);
        //    connection.Open();
        //    SqlDataReader reader = com.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        Subjects studentProfileView = new Subjects();
        //        studentProfileView.SubjecId = Convert.ToInt32(reader["SubjecId"]);
        //        studentProfileView.SubjectName = reader["SubjectName"].ToString();

        //        studentProfileViews.Add(studentProfileView);
        //    }
        //    reader.Close();
        //    connection.Close();
        //    return studentProfileViews;
        //}
        private string generateJwtToken(Tasks user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("TaskId", user.TaskId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
