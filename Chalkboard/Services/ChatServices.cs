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
    public interface IChatServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Chat> GetAll();
        List<Chat >GetById(int id);
       Chat SaveAll(Chat chat);
        string updateChat(Chat ct);
        string deleteChat(int id);
    }
    public class ChatServices: IChatServices
    {
        private List<Chat> _students = new List<Chat>
        {
            new Chat { ChatId = 1,taskId=1, ClassId = 1,SendById=1,
                SenderName="test", ChatDetails = "User", ChatTime = "test", IsRead = "yes",
                EntryDate = DateTime.Now,EntryBy = "Test",ProjectId =1,SchoolId = 1}
        };
        private readonly IConfiguration _configuration;

        private readonly AppSettings _appSettings;

        public ChatServices(IOptions<AppSettings> appSettings ,IConfiguration configuration)
        {
            _appSettings = appSettings.Value;


            _configuration = configuration;

        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.SenderName == model.Email && x.EntryBy == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public Chat SaveAll(Chat chat)
        {
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query =
                "INSERT INTO [dbo].[Chat]([TaskId],[ClassId],[SendById],[SenderName],[ChatDetails]," +
                "[ChatTime],[IsRead],[Deleted],[EntryDate],[EntryBy],[ProjectId],[SchoolId])" +
                "VALUES("+chat.taskId+","+chat.ChatId+ ","+chat.SendById+ ",'" + chat.SenderName+ "','" + chat.ChatDetails+ "'" +
                ",'" + chat.ChatTime+ "'," + chat.IsRead+ ",'False','"+DateTime.Now+"','"+chat.EntryBy+"',"+chat.ProjectId+","+chat.SchoolId+")";
            SqlCommand com=new SqlCommand(query,connection);
            connection.Open();
            var rowAffect=com.ExecuteNonQuery();
            connection.Close();
            if (rowAffect > 0)
            {
                return chat;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Chat> GetAll()
        {
            List<Chat> studentProfileViews = new List<Chat>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Chat where Deleted='False'";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Chat studentProfileView = new Chat();

                studentProfileView.ChatId = Convert.ToInt32(reader["ChatId"]);
                studentProfileView.taskId = Convert.ToInt32(reader["taskId"]);
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
                studentProfileView.SendById = Convert.ToInt32(reader["SendById"]);
                studentProfileView.SenderName = reader["SenderName"].ToString();
                studentProfileView.ChatDetails = reader["ChatDetails"].ToString();
                studentProfileView.ChatTime = reader["ChatTime"].ToString();
                studentProfileView.IsRead = reader["IsRead"].ToString();
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                studentProfileView.SchoolId = Convert.ToInt32(reader["SchoolId"]);
                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;
            //return _students;
        }

        public List<Chat> GetById(int taskId)
        {


            List<Chat> stdAttendances = new List<Chat>();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Chat where taskId=" + taskId + " and deleted='false'";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Chat stdAttendance = new Chat();
                stdAttendance.ChatId = Convert.ToInt32(reader["ChatId"]);
                stdAttendance.taskId = Convert.ToInt32(reader["taskId"]);
                stdAttendance.ClassId = Convert.ToInt32(reader["ClassId"]);
                stdAttendance.SendById = Convert.ToInt32(reader["SendById"]);
                stdAttendance.SenderName = reader["SenderName"].ToString();
                stdAttendance.ChatDetails = reader["ChatDetails"].ToString();
                stdAttendance.ChatTime = reader["ChatTime"].ToString();
                stdAttendance.IsRead = reader["IsRead"].ToString();
                stdAttendance.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                stdAttendance.EntryBy = reader["EntryBy"].ToString();
                stdAttendance.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                stdAttendance.SchoolId = Convert.ToInt32(reader["SchoolId"]);
                stdAttendances.Add(stdAttendance);

            }
            return stdAttendances;









            //Chat studentProfileView = new Chat();
            //string connectionString = _configuration.GetConnectionString("StudentDB");
            //SqlConnection connection = new SqlConnection(connectionString);
            //string query = "Select * FROM Chat where taskId=" + taskId + "";
            //SqlCommand com = new SqlCommand(query, connection);
            //connection.Open();
            //SqlDataReader reader = com.ExecuteReader();
            //while (reader.Read())
            //{
            //    studentProfileView.ChatId = Convert.ToInt32(reader["ChatId"]);
            //    studentProfileView.taskId = Convert.ToInt32(reader["taskId"]);
            //    studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
            //    studentProfileView.SendById = Convert.ToInt32(reader["SendById"]);
            //    studentProfileView.SenderName = reader["SenderName"].ToString();
            //    studentProfileView.ChatDetails = reader["ChatDetails"].ToString();
            //    studentProfileView.ChatTime = reader["ChatTime"].ToString();
            //    studentProfileView.IsRead = reader["IsRead"].ToString();
            //    studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
            //    studentProfileView.EntryBy = reader["EntryBy"].ToString();
            //    studentProfileView.ProjectId = Convert.ToInt32(reader["ProjectId"]);
            //    studentProfileView.SchoolId = Convert.ToInt32(reader["SchoolId"]);
            //}
            //return studentProfileView;

            //return _students.FirstOrDefault(x => x.taskId == id);
        }

        // helper methods

        private string generateJwtToken(Chat user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("taskId", user.taskId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string updateChat(Chat ct)
        {
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Update Chat SET   TaskId ="+ct.taskId+", ClassId ="+ct.ClassId+", SendById ="+ct.SendById+", SenderName ='"+ct.SenderName+"', ChatDetails ='"+ct.ChatDetails+"', ChatTime ='"+ct.ChatTime+"', IsRead ="+ct.IsRead+", EntryDate ='"+ct.EntryDate+"', EntryBy ='"+ct.EntryBy+"', ProjectId ="+ct.ProjectId+", SchoolId ="+ct.SchoolId+" where chatId="+ct.ChatId+"";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            var rowAffect=com.ExecuteNonQuery();
            if (rowAffect > 0)
            {
                return "Your Date Update Sucessfully";
            }
            else
            {
                return "Your Date Update Failed";
            }
        }

        public string deleteChat(int id)
        {
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);

            //if database value permanently delete then next line uncomment.
            //string query = "DELETE FROM Chat where chatId=" + id + "";
           
            //if database value don't permanently delete this data don't show client then next line uncomment.
            string query = "Update Chat SET Deleted ='True' where chatId=" + id + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            var rowAffect = com.ExecuteNonQuery();
            if (rowAffect > 0)
            {
                return "Your Date Delete Sucessfully";
            }
            else
            {
                return "Your Date Delete Failed";
            }
        }
    }
}
