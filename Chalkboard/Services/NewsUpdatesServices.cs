using ChalkboardAPI.Helpers;
using ChalkboardAPI.Models;
using Dapper;
using ESCHOOL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ESCHOOL.Services
{
    public interface INewsUpdatesServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<NewsUpdates> GetAll();
        List<NewsUpdates> GetNewsParamWiseData(int studentId);
        NewsUpdates GetById(int id);
    }

    public class NewsUpdatesServices: INewsUpdatesServices
    {
        private List<NewsUpdates> _students = new List<NewsUpdates>
        {
            new NewsUpdates { MsgID = 1,Headline="test", FullNews = "test",Msgfor="testc",
                PublishDate=DateTime.Now, NewsForTeacher = "User", NewsForStudent = "test", NewsForClass = "test",
                NewsForSection = "test",NewsForSubjectCode = "Test",SchoolId = 2,ProjectId = 2,EntryBy = "test" }
        };

        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public NewsUpdatesServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.Headline == model.Email && x.Msgfor == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<NewsUpdates> GetAll()
        {

            List<NewsUpdates> studentProfileViews = new List<NewsUpdates>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM NewsUpdates";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                NewsUpdates studentProfileView = new NewsUpdates();
                studentProfileView.MsgID = Convert.ToInt32(reader["MsgID"]);
                studentProfileView.Headline = reader["Headline"].ToString();
                studentProfileView.FullNews = reader["FullNews"].ToString();
                studentProfileView.Msgfor = reader["Msgfor"].ToString();
                studentProfileView.PublishDate = Convert.ToDateTime(reader["PublishDate"]);
                studentProfileView.NewsForTeacher = reader["NewsForTeacher"].ToString();
                studentProfileView.NewsForStudent = reader["NewsForStudent"].ToString();
                studentProfileView.NewsForClass = reader["NewsForClass"].ToString();
                studentProfileView.NewsForSection = reader["NewsForSection"].ToString();
                studentProfileView.NewsForSubjectCode = reader["NewsForSubjectCode"].ToString();
                studentProfileView.SchoolId = Convert.ToInt32(reader["SchoolId"]);
                studentProfileView.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
               
                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;

            //return _students;
        }

        public NewsUpdates GetById(int id)
        {

            NewsUpdates studentProfileView = new NewsUpdates();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM NewsUpdates where MsgID=" + id + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.MsgID = Convert.ToInt32(reader["MsgID"]);
                studentProfileView.Headline = reader["Headline"].ToString();
                studentProfileView.FullNews = reader["FullNews"].ToString();
                studentProfileView.Msgfor = reader["Msgfor"].ToString();
                studentProfileView.PublishDate = Convert.ToDateTime(reader["PublishDate"]);
                studentProfileView.NewsForTeacher = reader["NewsForTeacher"].ToString();
                studentProfileView.NewsForStudent = reader["NewsForStudent"].ToString();
                studentProfileView.NewsForClass = reader["NewsForClass"].ToString();
                studentProfileView.NewsForSection = reader["NewsForSection"].ToString();
                studentProfileView.NewsForSubjectCode = reader["NewsForSubjectCode"].ToString();
                studentProfileView.SchoolId = Convert.ToInt32(reader["SchoolId"]);
                studentProfileView.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
            }
            return studentProfileView;
            //return _students.FirstOrDefault(x => x.MsgID == id);
        }


        List<NewsUpdates> INewsUpdatesServices.GetNewsParamWiseData(int studentId)
        {
            List<NewsUpdates> _classwiseList = new List<NewsUpdates>();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            using (IDbConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<NewsUpdates>(@"SELECT Headline, FullNews, Msgfor, PublishDate FROM [dbo].[NewsUpdates] N 
                              INNER JOIN [dbo].[Students] S ON N.NewsForClass = S.ClassId  WHERE StudentID='" + studentId + "' AND N.SchoolId >0").ToList();
                
                //var oStudents = con.Query<NewsUpdates>(@"SELECT * FROM NewsUpdates WHERE NewsForStudent='" + studentId + "' AND NewsForStudent >0").ToList();
                
                if (oStudents != null && oStudents.Count() > 0)
                {
                    _classwiseList = oStudents;
                }
                con.Close();
            }
            return _classwiseList;
        }

        // helper methods

        private string generateJwtToken(NewsUpdates user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("MsgID", user.MsgID.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
