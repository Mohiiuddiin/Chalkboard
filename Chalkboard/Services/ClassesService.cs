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
    public interface IClassesService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Classes> GetAll();
        Classes GetById(int id);
    }
    public class ClassesService: IClassesService
    {
        private List<Classes> _students = new List<Classes>
        {
            new Classes { 
                ClassId = 1,

                 ClassName = "User",
                EntryBy = "test",
                EntryDate = DateTime.Now
                  }
        };

        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public ClassesService(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _appSettings = appSettings.Value;
            _configuration = configuration;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.ClassName == model.Email && x.EntryBy == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<Classes> GetAll()
        {

            List<Classes> studentProfileViews = new List<Classes>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Classes";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Classes studentProfileView = new Classes();
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
                studentProfileView.ClassName = reader["ClassName"].ToString();
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                
                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;

            //return _students;
        }

        public Classes GetById(int id)
        {
            Classes studentProfileView = new Classes();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Classes where ClassId=" + id + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
                studentProfileView.ClassName = reader["ClassName"].ToString();
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
            }
            return studentProfileView;

            //return _students.FirstOrDefault(x => x.ClassId == id);
        }

        // helper methods

        private string generateJwtToken(Classes user)
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
