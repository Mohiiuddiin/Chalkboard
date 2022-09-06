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
    public interface IStudentsService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Students> GetAll();
        Students GetById(string id);
    }
    public class StudentsServices: IStudentsService
    {


        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<Students> _students = new List<Students>
        {
            new Students { StudentId = 1,sl=1, ClassId=1,
                SectionId=1, DOB = "test",
                Email = "test@gmail.com",Password = "Test",EntryBy = "test",EntryDate = "test",StudentPhoto = "test" ,StudentNameE = "test",PhoneMobile = "test"}
        };
        private readonly IConfiguration _configuration;

        private readonly AppSettings _appSettings;

        public StudentsServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<Students> GetAll()
        {
            List<Students> studentProfileViews = new List<Students>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Students";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Students studentProfileView = new Students();
                studentProfileView.StudentId = Convert.ToInt32(reader["StudentId"]);
                studentProfileView.sl = Convert.ToInt32(reader["sl"]);
                //studentProfileView.RegNo = reader["RegNo"].ToString();
                studentProfileView.ClassId = Convert.ToInt32(reader["Class"]);
                studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);

                //studentProfileView.Name = reader["Name"].ToString();
                studentProfileView.DOB = reader["DOB"].ToString();
                studentProfileView.StudentNameE = reader["StudentNameE"].ToString();
                studentProfileView.PhoneMobile = reader["PhoneMobile"].ToString();
                //studentProfileView.MobNo = reader["MobNo"].ToString();
                studentProfileView.Email = reader["Email"].ToString();
                studentProfileView.Password = reader["Password"].ToString();
                //studentProfileView.PhotoId = reader["PhotoId"].ToString();
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.EntryDate = reader["EntryDate"].ToString();
                studentProfileView.StudentPhoto = reader["StudentPhoto"].ToString();
              
                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;


            //return _students;
        }

        public Students GetById(string Email)
        {
            Students studentProfileView = new Students();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Students where Email='" + Email + "'";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
               
                studentProfileView.StudentId = Convert.ToInt32(reader["StudentId"]);
                studentProfileView.sl = Convert.ToInt32(reader["sl"]);
                //studentProfileView.RegNo = reader["RegNo"].ToString();
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
                studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
                studentProfileView.StudentNameE = reader["StudentNameE"].ToString();
                studentProfileView.PhoneMobile = reader["PhoneMobile"].ToString();
                //studentProfileView.Name = reader["Name"].ToString();
                studentProfileView.DOB = reader["DOB"].ToString();
                //studentProfileView.MobNo = reader["MobNo"].ToString();
                studentProfileView.Email = reader["Email"].ToString();
                studentProfileView.Password = reader["Password"].ToString();
                //studentProfileView.PhotoId = reader["PhotoId"].ToString();
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.EntryDate = reader["EntryDate"].ToString();
                studentProfileView.StudentPhoto = reader["StudentPhoto"].ToString();
            }
            return studentProfileView;

            //return _students.FirstOrDefault(x => x.Email == id);
        }

        // helper methods

        private string generateJwtToken(Students user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Email", user.Email.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
