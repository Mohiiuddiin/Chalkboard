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
    public interface Ivw_BravoServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<vw_Bravo> GetAll();
        List< vw_Bravo> GetById(string id);
    }
    public class vw_BravoServices: Ivw_BravoServices
    {

        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<vw_Bravo> _students = new List<vw_Bravo>
        {
            new vw_Bravo { ExamName = "test",StudentId="1",
                SubjectId="1", SubjectName = "User", GPA = "test", ResultMarks = "test"
                 }
        };

        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public vw_BravoServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.StudentId == model.Email && x.SubjectId == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<vw_Bravo> GetAll()
        {

            List<vw_Bravo> studentProfileViews = new List<vw_Bravo>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM vw_Bravo";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                vw_Bravo studentProfileView = new vw_Bravo();
                studentProfileView.ExamName = reader["ExamName"].ToString();
                studentProfileView.StudentId = reader["StudentId"].ToString();
                studentProfileView.SubjectId = reader["SubjectId"].ToString();
                studentProfileView.SubjectName = reader["SubjectName"].ToString();
                studentProfileView.GPA = reader["GPA"].ToString();
                studentProfileView.ResultMarks = reader["ResultMarks"].ToString();
                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;

            //return _students;
        }

        public List< vw_Bravo >GetById(string id)
        {

            List<vw_Bravo> stdAttendances = new List<vw_Bravo>();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM vw_Bravo where StudentId=" + id + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                vw_Bravo stdAttendance = new vw_Bravo();
                stdAttendance.ExamName = reader["ExamName"].ToString();
                stdAttendance.StudentId = reader["StudentId"].ToString();
                stdAttendance.SubjectId = reader["SubjectId"].ToString();
                stdAttendance.SubjectName = reader["SubjectName"].ToString();
                stdAttendance.GPA = reader["GPA"].ToString();
                stdAttendance.ResultMarks = reader["ResultMarks"].ToString();
               
                stdAttendances.Add(stdAttendance);

            }
            return stdAttendances;

            //List<  vw_Bravo> studentProfileView = new List< vw_Bravo>();

            //  string connectionString = _configuration.GetConnectionString("StudentDB");
            //  SqlConnection connection = new SqlConnection(connectionString);
            //  string query = "Select * FROM vw_Bravo where SubjectId=" + id + "";
            //  SqlCommand com = new SqlCommand(query, connection);
            //  connection.Open();
            //  SqlDataReader reader = com.ExecuteReader();
            //  while (reader.Read())
            //  {
            //      vw_Bravo studentProfileView = new vw_Bravo();

            //      studentProfileView.ExamName = reader["ExamName"].ToString();
            //      studentProfileView.StudentId = reader["StudentId"].ToString();
            //      studentProfileView.SubjectId = reader["SubjectId"].ToString();
            //      studentProfileView.SubjectName = reader["SubjectName"].ToString();
            //      studentProfileView.GPA = reader["GPA"].ToString();
            //      studentProfileView.ResultMarks = reader["ResultMarks"].ToString();
            //      studentProfileView.Add(studentProfileView);

            //  }
            //  return studentProfileView;


            //return _students.FirstOrDefault(x => x.SubjectId ==  id);
        }

        // helper methods

        private string generateJwtToken(vw_Bravo user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("StudentId", user.StudentId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
