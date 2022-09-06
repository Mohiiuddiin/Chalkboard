using ChalkboardAPI.Helpers;
using ChalkboardAPI.Models;
using ESCHOOL.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChalkboardAPI.Services
{
    public interface ITeachersServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Teachers> GetAll();
        Teachers GetById(int id);
    }


    public class TeachersServices: ITeachersServices
    {
        private List<Teachers> _students = new List<Teachers>
        {
            new Teachers { TeacherId = 1, TeacherName = "test",
                 MobileNo = "User", Email = "test@gmail.com", Password = "123456",
                PhotoId = "Test",EntryBy = "Test",EntryDate = "test",SchoolId=1,TeacherRolI="test" }
        };
        private readonly AppSettings _appSettings;

        public TeachersServices(IOptions<AppSettings> appSettings)
        {
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
        public IEnumerable<Teachers> GetAll()
        {
            return _students;
        }

        public Teachers GetById(int id)
        {
            return _students.FirstOrDefault(x => x.TeacherId == id);
        }

        // helper methods

        private string generateJwtToken(Teachers user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("TeacherId", user.TeacherId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
