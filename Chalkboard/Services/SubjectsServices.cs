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

namespace ChalkboardAPI.Services
{
    public interface ISubjectsServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Subjects> GetAll();
        List<Subjects> GetById(int id);
    }
    public class SubjectsServices : ISubjectsServices
    {
        private List<Subjects> _students = new List<Subjects>
        {
            new Subjects { SubjecId = 1,ClassId=1, SubjectName = "test",BookSt="Bangla",
                EntryDate=DateTime.Now, SchoolId = 1, TeacherID = 1, SubjectCode = "test",Section=1
                 }
        };

        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public SubjectsServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.BookSt == model.Email && x.SubjectName == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<Subjects> GetAll()
        {

            List<Subjects> studentProfileViews = new List<Subjects>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Subjects";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Subjects studentProfileView = new Subjects();
                studentProfileView.SubjecId = Convert.ToInt32(reader["SubjecId"]);
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
                //studentProfileView.RegNo = reader["RegNo"].ToString();
                studentProfileView.TeacherID = Convert.ToInt32(reader["TeacherID"]);
                studentProfileView.SchoolId = Convert.ToInt32(reader["SchoolId"]);
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                studentProfileView.Section = Convert.ToInt32(reader["Section"]);


                //studentProfileView.Name = reader["Name"].ToString();
                studentProfileView.SubjectName = reader["SubjectName"].ToString();
                studentProfileView.BookSt = reader["BookSt"].ToString();
                studentProfileView.SubjectCode = reader["SubjectCode"].ToString();

                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;
            //return _students;
        }

        public List<Subjects> GetById(int id)
        {


            List<Subjects> stdAttendances = new List<Subjects>();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Subjects where ClassId=" + id + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Subjects stdAttendance = new Subjects();
                stdAttendance.SubjecId = Convert.ToInt32(reader["SubjecId"]);
                stdAttendance.ClassId = Convert.ToInt32(reader["ClassId"]);
                //studentProfileView.RegNo = reader["RegNo"].ToString();
                stdAttendance.TeacherID = Convert.ToInt32(reader["TeacherID"]);
                stdAttendance.SchoolId = Convert.ToInt32(reader["SchoolId"]);
                stdAttendance.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                stdAttendance.Section = Convert.ToInt32(reader["Section"]);


                //studentProfileView.Name = reader["Name"].ToString();
                stdAttendance.SubjectName = reader["SubjectName"].ToString();
                stdAttendance.BookSt = reader["BookSt"].ToString();
                stdAttendance.SubjectCode = reader["SubjectCode"].ToString();
                stdAttendances.Add(stdAttendance);

            }
            return stdAttendances;


            //List<Subjects> studentProfileView = new List<Subjects>();

            //  string connectionString = _configuration.GetConnectionString("StudentDB");
            //  SqlConnection connection = new SqlConnection(connectionString);
            //  string query = "Select * FROM Subjects where ClassId='" + id + "'";
            //  SqlCommand com = new SqlCommand(query, connection);
            //  connection.Open();
            //  SqlDataReader reader = com.ExecuteReader();
            //  while (reader.Read())
            //  {
            //      Subjects studentProfileView = new Subjects();

            //      /// Subjects studentProfileView = new Subjects();
            //      studentProfileView.SubjecId = Convert.ToInt32(reader["SubjecId"]);
            //      studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
            //      //studentProfileView.RegNo = reader["RegNo"].ToString();
            //      studentProfileView.TeacherID = Convert.ToInt32(reader["TeacherID"]);
            //      studentProfileView.SchoolId = Convert.ToInt32(reader["SchoolId"]);
            //      studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
            //      studentProfileView.Section = Convert.ToInt32(reader["Section"]);


            //      //studentProfileView.Name = reader["Name"].ToString();
            //      studentProfileView.SubjectName = reader["SubjectName"].ToString();
            //      studentProfileView.BookSt = reader["BookSt"].ToString();
            //      studentProfileView.SubjectCode = reader["SubjectCode"].ToString();
            //      studentProfileView.Add(studentProfileView);

            //  }
            //  return studentProfileView;

            //return _students.FirstOrDefault(x => x.SubjecId == id);
        }


        // helper methods
        private string generateJwtToken(Subjects user)
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
