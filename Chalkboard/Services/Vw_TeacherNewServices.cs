using Dapper;
using ChalkboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ChalkboardAPI.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ESCHOOL.Models;
using Microsoft.Extensions.Configuration;

namespace ChalkboardAPI.Services
{


    public interface IVw_TeacherNewServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Vw_TeacherNew> GetAll();
       List< Vw_TeacherNew> GetById(int id);
    }
    public class Vw_TeacherNewServices : IVw_TeacherNewServices
    {



        private List<Vw_TeacherNew> _students = new List<Vw_TeacherNew>
        {
            new Vw_TeacherNew { SubjectId = 1,SectionId=1, SubjectName = "test",BookSt="test",
                TeacherName="User", MobileNo = "123456", Email = "test@gmail.com", TeacherId = 1,
                ClassesId = 1}
        };

        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public Vw_TeacherNewServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.Email == model.Email && x.MobileNo == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<Vw_TeacherNew> GetAll()
        {
            List<Vw_TeacherNew> studentProfileViews = new List<Vw_TeacherNew>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Vw_TeacherNew";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Vw_TeacherNew studentProfileView = new Vw_TeacherNew();
                studentProfileView.SubjectId = Convert.ToInt32(reader["SubjectId"]);
                studentProfileView.SubjectName = reader["SubjectName"].ToString();
                studentProfileView.BookSt = reader["BookSt"].ToString();
                studentProfileView.TeacherName = reader["TeacherName"].ToString();
                studentProfileView.MobileNo = reader["MobileNo"].ToString();
                studentProfileView.Email = reader["Email"].ToString();
                studentProfileView.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                studentProfileView.ClassesId = Convert.ToInt32(reader["ClassesId"]);
                studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);

                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;


            //return _students;
        }

        public List <Vw_TeacherNew>GetById(int StudentId)
        {
            List<Vw_TeacherNew> StdTeacher = new List<Vw_TeacherNew>();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Vw_TeacherNew where StudentID=" + StudentId + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Vw_TeacherNew TeacherSectionwise = new Vw_TeacherNew();
                TeacherSectionwise.SubjectId = Convert.ToInt32(reader["SubjectId"]);
                TeacherSectionwise.SubjectName = reader["SubjectName"].ToString();
                TeacherSectionwise.BookSt = reader["BookSt"].ToString();
                TeacherSectionwise.TeacherName = reader["TeacherName"].ToString();
                TeacherSectionwise.MobileNo = reader["MobileNo"].ToString();
                TeacherSectionwise.Email = reader["Email"].ToString();
                TeacherSectionwise.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                TeacherSectionwise.ClassesId = Convert.ToInt32(reader["ClassesId"]);
                TeacherSectionwise.SectionId = Convert.ToInt32(reader["SectionId"]);
                StdTeacher.Add(TeacherSectionwise);

            }
            return StdTeacher;
        }


        public List<Vw_TeacherNew> GetByClassesId(int ClassesId)
        {
            List<Vw_TeacherNew> stdAttendances = new List<Vw_TeacherNew>();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Vw_TeacherNew where ClassesId=" + ClassesId + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Vw_TeacherNew stdAttendance = new Vw_TeacherNew();
                stdAttendance.SubjectId = Convert.ToInt32(reader["SubjectId"]);
                stdAttendance.SubjectName = reader["SubjectName"].ToString();
                stdAttendance.BookSt = reader["BookSt"].ToString();
                stdAttendance.TeacherName = reader["TeacherName"].ToString();
                stdAttendance.MobileNo = reader["MobileNo"].ToString();
                stdAttendance.Email = reader["Email"].ToString();
                stdAttendance.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                stdAttendance.ClassesId = Convert.ToInt32(reader["ClassesId"]);
                stdAttendance.SectionId = Convert.ToInt32(reader["SectionId"]);
                stdAttendances.Add(stdAttendance);

            }
            return stdAttendances;



            //List< Vw_TeacherNew> studentProfileView = new List< Vw_TeacherNew>();

            // string connectionString = _configuration.GetConnectionString("StudentDB");
            // SqlConnection connection = new SqlConnection(connectionString);
            // string query = "Select * FROM Vw_TeacherNew where ClassesId=" + ClassesId + "";
            // SqlCommand com = new SqlCommand(query, connection);
            // connection.Open();
            // SqlDataReader reader = com.ExecuteReader();
            // while (reader.Read())
            // {
            //     Vw_TeacherNew studentProfileView = new Vw_TeacherNew();


            //     studentProfileView.SubjectId = Convert.ToInt32(reader["SubjectId"]);
            //     studentProfileView.SubjectName = reader["SubjectName"].ToString();
            //     studentProfileView.BookSt = reader["BookSt"].ToString();
            //     studentProfileView.TeacherName = reader["TeacherName"].ToString();
            //     studentProfileView.MobileNo = reader["MobileNo"].ToString();
            //     studentProfileView.Email = reader["Email"].ToString();
            //     studentProfileView.TeacherId = Convert.ToInt32(reader["TeacherId"]);
            //     studentProfileView.ClassesId = Convert.ToInt32(reader["ClassesId"]);
            //     studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
            //     studentProfileView.Add(studentProfileView);

            // }
            // return studentProfileView;


            //return _students.FirstOrDefault(x => x.ClassesId == id);
        }

        // helper methods

        private string generateJwtToken(Vw_TeacherNew user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("ClassesId", user.ClassesId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }











        //List<Vw_TeacherNew> _Vw_TeacherNewServicesList = new List<Vw_TeacherNew>();
        //public List<Vw_TeacherNew> Get(int Id)
        //{
        //    _Vw_TeacherNewServicesList = new List<Vw_TeacherNew>();

        //    using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
        //    {
        //        if (con.State == ConnectionState.Closed) con.Open();
        //        var oStudents = con.Query<Vw_TeacherNew>(@"SELECT * FROM Vw_TeacherNew WHERE ClassesId='" + Id + "' ").ToList();

        //        if (oStudents != null && oStudents.Count() > 0)
        //        {
        //            _Vw_TeacherNewServicesList = oStudents;
        //        }
        //        con.Close();
        //    }
        //    return _Vw_TeacherNewServicesList;
        //}

        //public List<Vw_TeacherNew> Gets()
        //{
        //    _Vw_TeacherNewServicesList = new List<Vw_TeacherNew>();

        //    using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
        //    {
        //        if (con.State == ConnectionState.Closed) con.Open();
        //        var oStudents = con.Query<Vw_TeacherNew>(@"SELECT * FROM Vw_TeacherNew ").ToList();

        //        if (oStudents != null && oStudents.Count() > 0)
        //        {
        //            _Vw_TeacherNewServicesList = oStudents;
        //        }
        //        con.Close();
        //    }
        //    return _Vw_TeacherNewServicesList;
        //}
    }
}
