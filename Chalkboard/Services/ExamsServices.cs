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
    public interface IExamsServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Exams> GetAll();
        Exams GetById(int id);
    }
    public class ExamsServices: IExamsServices
    {
        private List<Exams> _students = new List<Exams>
        {
            new Exams { ExamId = 1,ClassId=1, ExamName = "testc",StartingDate=DateTime.Now,
                ResultDate=DateTime.Now, EntryBy = "User", EntryDate = DateTime.Now }
        };
        private readonly IConfiguration _configuration;

        private readonly AppSettings _appSettings;

        public ExamsServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _appSettings = appSettings.Value;


            _configuration = configuration;

        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.EntryBy == model.Email && x.ExamName == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<Exams> GetAll()
        {

            List<Exams> studentProfileViews = new List<Exams>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Exams";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Exams studentProfileView = new Exams();

                studentProfileView.ExamId = Convert.ToInt32(reader["ExamId"]);
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
               
                studentProfileView.ExamName = reader["ExamName"].ToString();
                studentProfileView.StartingDate = Convert.ToDateTime(reader["StartingDate"]);
                studentProfileView.ResultDate = Convert.ToDateTime(reader["ResultDate"]);
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
               


                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;
            //return _students;
        }

        public Exams GetById(int taskId)
        {
            Exams studentProfileView = new Exams();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Exams where ClassId=" + taskId + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.ExamId = Convert.ToInt32(reader["ExamId"]);
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);

                studentProfileView.ExamName = reader["ExamName"].ToString();
                studentProfileView.StartingDate = Convert.ToDateTime(reader["StartingDate"]);
                studentProfileView.ResultDate = Convert.ToDateTime(reader["ResultDate"]);
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
            }
            return studentProfileView;

            //return _students.FirstOrDefault(x => x.taskId == id);
        }

        // helper methods

        private string generateJwtToken(Exams user)
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
