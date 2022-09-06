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
    public interface IVw_StudentProfileViewServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Vw_StudentProfileView> GetAll();
       List< Vw_StudentProfileView> GetById(int id);
    }
    public class Vw_StudentProfileViewServices: IVw_StudentProfileViewServices
    {
        private List<Vw_StudentProfileView> _students = new List<Vw_StudentProfileView>
        {
            new Vw_StudentProfileView { sl = 1,StudentID="1", StudentNameE = "test",Class="testc",
                FatherNameE="testc", MotherNameE = "User", DOB = "05-09-2021", StudentPhoto = "test",
                BloodGroup = "Test",RollNumber = "Test",Email = "test",SectionId = 1,SectionName = "test",name = "test",Gender = "test",ClassId = "test" }
        };

        private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;

        public Vw_StudentProfileViewServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _appSettings = appSettings.Value;
            _configuration = configuration;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.Email == model.Email && x.ClassId == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<Vw_StudentProfileView> GetAll()
        {
            List<Vw_StudentProfileView> studentProfileViews = new List<Vw_StudentProfileView>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Vw_StudentProfileView";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Vw_StudentProfileView studentProfileView = new Vw_StudentProfileView();
                studentProfileView.sl = Convert.ToInt32(reader["sl"]);
                studentProfileView.BloodGroup = reader["BloodGroup"].ToString();
                studentProfileView.Email = reader["Email"].ToString();
                studentProfileView.Class = reader["Class"].ToString();
                studentProfileView.ClassId = reader["ClassId"].ToString();
                studentProfileView.DOB = reader["DOB"].ToString();
                studentProfileView.FatherNameE = reader["FatherNameE"].ToString();
                studentProfileView.Gender = reader["Gender"].ToString();
                studentProfileView.MotherNameE = reader["MotherNameE"].ToString();
                studentProfileView.name = reader["name"].ToString();
                studentProfileView.RollNumber = reader["RollNumber"].ToString();
                studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
                studentProfileView.SectionName = reader["SectionName"].ToString();
                studentProfileView.StudentID = reader["StudentId"].ToString();
                studentProfileView.StudentNameE = reader["StudentNameE"].ToString();
                studentProfileView.StudentPhoto = reader["StudentPhoto"].ToString();
                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;
        }


        public List< Vw_StudentProfileView> GetById(int stdid)
        {

            List<Vw_StudentProfileView> stdAttendances = new List<Vw_StudentProfileView>();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Vw_StudentProfileView where StudentID='" + stdid + "'";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Vw_StudentProfileView stdAttendance = new Vw_StudentProfileView();
                stdAttendance.sl = Convert.ToInt32(reader["sl"]);
                stdAttendance.BloodGroup = reader["BloodGroup"].ToString();
                stdAttendance.Email = reader["Email"].ToString();
                stdAttendance.Class = reader["Class"].ToString();
                stdAttendance.ClassId = reader["ClassId"].ToString();
                stdAttendance.DOB = reader["DOB"].ToString();
                stdAttendance.FatherNameE = reader["FatherNameE"].ToString();
                stdAttendance.Gender = reader["Gender"].ToString();
                stdAttendance.MotherNameE = reader["MotherNameE"].ToString();
                stdAttendance.name = reader["name"].ToString();
                stdAttendance.RollNumber = reader["RollNumber"].ToString();
                stdAttendance.SectionId = Convert.ToInt32(reader["SectionId"]);
                stdAttendance.SectionName = reader["SectionName"].ToString();
                stdAttendance.StudentID = reader["StudentId"].ToString();
                stdAttendance.StudentNameE = reader["StudentNameE"].ToString();
                stdAttendance.StudentPhoto = reader["StudentPhoto"].ToString();
                stdAttendances.Add(stdAttendance);

            }
            return stdAttendances;




            //Vw_StudentProfileView studentProfileView = new Vw_StudentProfileView();

            //string connectionString = _configuration.GetConnectionString("StudentDB");
            //SqlConnection connection = new SqlConnection(connectionString);
            //string query = "Select * FROM Vw_StudentProfileView where sl="+sl+"";
            //SqlCommand com = new SqlCommand(query, connection);
            //connection.Open();
            //SqlDataReader reader = com.ExecuteReader();
            //while (reader.Read())
            //{
            //    studentProfileView.BloodGroup = reader["BloodGroup"].ToString();
            //    studentProfileView.Email = reader["Email"].ToString();
            //    studentProfileView.Class = reader["Class"].ToString();
            //    studentProfileView.ClassId = reader["ClassId"].ToString();
            //    studentProfileView.DOB = reader["DOB"].ToString();
            //    studentProfileView.FatherNameE = reader["FatherNameE"].ToString();
            //    studentProfileView.Gender = reader["Gender"].ToString();
            //    studentProfileView.MotherNameE = reader["MotherNameE"].ToString();
            //    studentProfileView.name = reader["name"].ToString();
            //    studentProfileView.RollNumber = reader["RollNumber"].ToString();
            //    studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
            //        //Convert.ToDateTime(reader["Date"]);
            //    studentProfileView.SectionName = reader["SectionName"].ToString();
            //    studentProfileView.StudentID = reader["StudentId"].ToString();
            //    studentProfileView.StudentNameE = reader["StudentNameE"].ToString();
            //    studentProfileView.StudentPhoto = reader["StudentPhoto"].ToString();
            //}
            //return studentProfileView;
        }

        // helper methods

        private string generateJwtToken(Vw_StudentProfileView user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("sl", user.sl.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
