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

    public interface ISectionsServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Sections> GetAll();
        Sections GetById(int id);
    }
    public class SectionsServices: ISectionsServices
    {
        private List<Sections> _students = new List<Sections>
        {
            new Sections { SectionId = 1,ClassId=1, SectionName = "test",EntryBy="test",
                EntryDate=DateTime.Now }
        };

        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public SectionsServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.SectionName == model.Email && x.EntryBy == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<Sections> GetAll()
        {

            List<Sections> studentProfileViews = new List<Sections>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Sections";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Sections studentProfileView = new Sections();
                studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
                studentProfileView.SectionName = reader["SectionName"].ToString();
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
               
                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;

            //return _students;
        }

        public Sections GetById(int id)
        {

            Sections studentProfileView = new Sections();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM Sections where ClassId=" + id + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
                studentProfileView.ClassId = Convert.ToInt32(reader["ClassId"]);
                studentProfileView.SectionName = reader["SectionName"].ToString();
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
            }
            return studentProfileView;
            //return _students.FirstOrDefault(x => x.MsgID == id);
        }

        // helper methods

        private string generateJwtToken(Sections user)
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
