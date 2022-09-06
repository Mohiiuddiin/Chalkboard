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
    public interface ITaskChatsServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<TaskChats> GetAll();
        TaskChats GetById(int id);
    }
    public class TaskChatsServices: ITaskChatsServices
    {
        private List<TaskChats> _students = new List<TaskChats>
        {
            new TaskChats { ChatId = 1,ParentChatId=1, TaskId = 1,TeacherId=1,
                StudentId=1, CommentDetail = "User", EntryDate = DateTime.Now, EntryBy = "yes"
                }
        };
        private readonly IConfiguration _configuration;

        private readonly AppSettings _appSettings;

        public TaskChatsServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _appSettings = appSettings.Value;


            _configuration = configuration;

        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.CommentDetail == model.Email && x.EntryBy == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<TaskChats> GetAll()
        {

            List<TaskChats> studentProfileViews = new List<TaskChats>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM TaskChats";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                TaskChats studentProfileView = new TaskChats();

                studentProfileView.ChatId = Convert.ToInt32(reader["ChatId"]);
                studentProfileView.ParentChatId = Convert.ToInt32(reader["ParentChatId"]);
                studentProfileView.TaskId = Convert.ToInt32(reader["TaskId"]);
                studentProfileView.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                studentProfileView.CommentDetail = reader["CommentDetail"].ToString();
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
               


                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;
            //return _students;
        }

        public TaskChats GetById(int taskId)
        {
            TaskChats studentProfileView = new TaskChats();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM TaskChats where taskId=" + taskId + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.ChatId = Convert.ToInt32(reader["ChatId"]);
                studentProfileView.ParentChatId = Convert.ToInt32(reader["ParentChatId"]);
                studentProfileView.TaskId = Convert.ToInt32(reader["TaskId"]);
                studentProfileView.TeacherId = Convert.ToInt32(reader["TeacherId"]);
                studentProfileView.CommentDetail = reader["CommentDetail"].ToString();
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
            }
            return studentProfileView;

            //return _students.FirstOrDefault(x => x.taskId == id);
        }

        // helper methods

        private string generateJwtToken(TaskChats user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("TaskId", user.TaskId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
