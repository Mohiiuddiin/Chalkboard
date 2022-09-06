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
    public interface IVw_TimeTableServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Vw_TimeTable> GetAll();
        List<Vw_TimeTable> GetById(int id);
    }
    public class Vw_TimeTableServices: IVw_TimeTableServices
    {
        private List<Vw_TimeTable> _students = new List<Vw_TimeTable>
        {
            new Vw_TimeTable { Day = "san",Date=DateTime.Now, StartTime = "test",EndTime="test",
                 SubjectName = "User", SubjectId = 1, ClassId = 1,
                ClassesId = 1,SectionId = 1}
        };

        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public Vw_TimeTableServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.StartTime == model.Email && x.EndTime == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<Vw_TimeTable> GetAll()
        {

            List<Vw_TimeTable> studentProfileViews = new List<Vw_TimeTable>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Vw_TimeTable";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Vw_TimeTable studentProfileView = new Vw_TimeTable();
                studentProfileView.Day = reader["Day"].ToString();
                studentProfileView.Date = Convert.ToDateTime(reader["Date"]);

                studentProfileView.StartTime = reader["StartTime"].ToString();
                studentProfileView.EndTime = reader["EndTime"].ToString();
                studentProfileView.SubjectName = reader["SubjectName"].ToString();
                studentProfileView.SubjectId = Convert.ToInt32(reader["SubjectId"]);
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
                studentProfileView.ClassesId = Convert.ToInt32(reader["ClassesId"]);
                studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;



            //return _students;
        }

        public List <Vw_TimeTable> GetById(int id)
        {



            List<Vw_TimeTable> stdAttendances = new List<Vw_TimeTable>();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Vw_TimeTable where ClassId=" + id + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Vw_TimeTable stdAttendance = new Vw_TimeTable();
                stdAttendance.Day = reader["Day"].ToString();
                //stdAttendance.Date = !string.IsNullOrEmpty(reader["Date"].ToString()) ? Convert.ToDateTime(reader["Date"]) : Convert.ToDateTime(DBNull.Value);
                stdAttendance.Date = reader["Date"] == DBNull.Value ? default : Convert.ToDateTime(reader["Date"]);

                stdAttendance.StartTime = reader["StartTime"].ToString();
                stdAttendance.EndTime = reader["EndTime"].ToString();
                stdAttendance.SubjectName = reader["SubjectName"].ToString();
                stdAttendance.SubjectId = Convert.ToInt32(reader["SubjectId"]);
                stdAttendance.ClassId = Convert.ToInt32(reader["ClassId"]);
                stdAttendance.ClassesId = Convert.ToInt32(reader["ClassesId"]);
                stdAttendance.SectionId = Convert.ToInt32(reader["SectionId"]);
                stdAttendances.Add(stdAttendance);

            }
            return stdAttendances;

            //List <Vw_TimeTable> vw_TimeTable = new List <Vw_TimeTable>();

            //string connectionString = _configuration.GetConnectionString("StudentDB");
            //SqlConnection connection = new SqlConnection(connectionString);
            //string query = "Select * FROM Vw_TimeTable where ClassId=" + id + "";
            //SqlCommand com = new SqlCommand(query, connection);
            //connection.Open();
            //SqlDataReader reader = com.ExecuteReader();
            //while (reader.Read())
            //{
            //    Vw_TimeTable vw_TimeTable = new Vw_TimeTable();

            //    vw_TimeTable.Day = reader["Day"].ToString();
            //    vw_TimeTable.Date = Convert.ToDateTime(reader["Date"]);

            //    vw_TimeTable.StartTime = reader["StartTime"].ToString();
            //    vw_TimeTable.EndTime = reader["EndTime"].ToString();
            //    vw_TimeTable.SubjectName = reader["SubjectName"].ToString();
            //    vw_TimeTable.SubjectId = Convert.ToInt32(reader["SubjectId"]);
            //    vw_TimeTable.ClassId = Convert.ToInt32(reader["ClassId"]);
            //    vw_TimeTable.ClassesId = Convert.ToInt32(reader["ClassesId"]);
            //    vw_TimeTable.SectionId = Convert.ToInt32(reader["SectionId"]);
            //    vw_TimeTable.Add(vw_TimeTable);

            //}
            //return vw_TimeTable;
            //return _students.FirstOrDefault(x => x.ClassId == id);
        }

        // helper methods

        private string generateJwtToken(Vw_TimeTable user)
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
