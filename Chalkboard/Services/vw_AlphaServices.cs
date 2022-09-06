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
    public interface Ivw_AlphaServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<vw_Alpha> GetAll();
        IEnumerable<vw_Alpha> GetStudentTaskData(int id);
        vw_Alpha GetById(int id);
      //  vw_Alpha SaveAll(vw_Alpha chat);
    }
    public class vw_AlphaServices: Ivw_AlphaServices
    {
        private List<vw_Alpha> _students = new List<vw_Alpha>
        {
            new vw_Alpha { TaskId = "1",TaskDetails="test", TaskDate = "10-10-2021",TaskTypeName="test",
                SubjectName="test", SectionName = "User", TeacherId = 1, ClassId = 1,
                EntryBy="test",TeacherName = "Test"}
        };
        private readonly IConfiguration _configuration;

        private readonly AppSettings _appSettings;

        public vw_AlphaServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _appSettings = appSettings.Value;


            _configuration = configuration;

        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.EntryBy == model.Email && x.SubjectName == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<vw_Alpha> GetAll()
        {

            List<vw_Alpha> studentProfileViews = new List<vw_Alpha>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM vw_Alpha";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                vw_Alpha studentProfileView = new vw_Alpha();
                studentProfileView.TaskId = reader["TaskId"].ToString();
                studentProfileView.TaskDetails = reader["TaskDetails"].ToString();
                studentProfileView.TaskDate = reader["TaskDate"].ToString();
                studentProfileView.TaskTypeName = reader["TaskTypeName"].ToString();
                studentProfileView.SubjectName = reader["SubjectName"].ToString();
                studentProfileView.SectionName = reader["SectionName"].ToString();
                studentProfileView.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);

                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.TeacherName = reader["TeacherName"].ToString();
             

                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;

            //return _students;
        }

        public vw_Alpha GetById(int id)
        {
            vw_Alpha studentProfileView = new vw_Alpha();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM vw_Alpha where ClassId=" + id + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.TaskId = reader["TaskId"].ToString();
                studentProfileView.TaskDetails = reader["TaskDetails"].ToString();
                studentProfileView.TaskDate = reader["TaskDate"].ToString();
                studentProfileView.TaskTypeName = reader["TaskTypeName"].ToString();
                studentProfileView.SubjectName = reader["SubjectName"].ToString();
                studentProfileView.SectionName = reader["SectionName"].ToString();
                studentProfileView.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);

                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.TeacherName = reader["TeacherName"].ToString();
            }
            return studentProfileView;

            //return _students.FirstOrDefault(x => x.ClassId == id);
        }


        public IEnumerable<vw_Alpha> GetStudentTaskData(int id)
        {
            List<vw_Alpha> studentProfileViews = new List<vw_Alpha>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT * FROM vw_StudentTaskData WHERE StudentID = '" + id + "'";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                vw_Alpha studentProfileView = new vw_Alpha();
                studentProfileView.TaskId = reader["TaskId"].ToString();
                studentProfileView.TaskDetails = reader["TaskDetails"].ToString();
                studentProfileView.TaskDate = reader["TaskDate"].ToString();
                studentProfileView.TaskTypeName = reader["TaskTypeName"].ToString();
                studentProfileView.SubjectName = reader["SubjectName"].ToString();
                studentProfileView.SectionName = reader["SectionName"].ToString();
                studentProfileView.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);

                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.TeacherName = reader["TeacherName"].ToString();

                //studentProfileView.Students.StudentId = Convert.ToInt32(reader["StudentID"]);
                //studentProfileView.Students.SectionId = Convert.ToInt32(reader["SectionId"]);

                studentProfileViews.Add(studentProfileView);
            }

            reader.Close();
            connection.Close();
            return studentProfileViews;
        }


        // helper methods
        private string generateJwtToken(vw_Alpha user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("ClassId", user.ClassId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
