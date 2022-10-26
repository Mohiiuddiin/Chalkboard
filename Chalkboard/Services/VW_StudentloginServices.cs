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

namespace ChalkboardAPI.Services
{
    public interface IVW_StudentLoginServicese
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        AuthenticateResponse GuardianAuthenticate(AuthenticateRequest model);
        Task<bool> UpdateDeviceId(VW_StudentLogin entity);//Update API
        Task<bool> UpdateBearerTokenOnDB(string token, DateTime? tokenExp, int stdId);
        Task<bool> RemoveStdDeviceId(VW_StudentLogin entity);//Remove DeviceID Update API
        Task<bool> UpdateGuardianDeviceId(VW_StudentLogin entity);//Update API
        Task<bool> RemoveGuardianDeviceId(VW_StudentLogin entity);//Remove DeviceID Update API
        IEnumerable<VW_StudentLogin> GetAll();
        VW_StudentLogin GetById(int StudentloginId);
        int InsertLoginHistory(LoginHistory loginHistory);
    }
    public class VW_StudentLoginServicese : IVW_StudentLoginServicese
    {
        private List<VW_StudentLogin> _studentlogin = new List<VW_StudentLogin>
        {
            new VW_StudentLogin {
                StudentId = 1,
                Email = "md.mohiiuddiin@gmail.com",
                Password = "12345678"
            }
        };

        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;
        public VW_StudentLoginServicese(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = GetStudentCheck(model.Email, model.Password);

            //var user = _studentlogin.SingleOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var token = "";

            if (user.BearerToken != null && DateTime.Now<=user.BearerTokenExp)
            {
                token = user.BearerToken;                
            }
            else
            {
                token = generateJwtToken(user);
                _ = UpdateBearerTokenOnDB(token, DateTime.Now.AddDays(365), user.StudentId);
            }
            return new AuthenticateResponse(user, token);
        }
        public int InsertLoginHistory(LoginHistory loginHistory)
        {
            try
            {
                //LoginHistory loginHistoryToInsert = new LoginHistory();

                string connectionString = _configuration.GetConnectionString("StudentDB");
                //SELECT Email,GuardianEmail
                //FROM[ESCHOOL].[dbo].[Students] where PhoneMobile = '' or Email = ''
                if (!loginHistory.MemberID.Contains('@'))
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        if (loginHistory.IsStudentOrParent == 6)
                        {
                            loginHistory.MemberID = connection.Query<string>("SELECT Email FROM[ESCHOOL].[dbo].[Students] where PhoneMobile = '" + loginHistory.MemberID + "' or Email = '" + loginHistory.MemberID + "'").FirstOrDefault();

                        }
                        else if (loginHistory.IsStudentOrParent == 5)
                        {
                            loginHistory.MemberID = connection.Query<string>("SELECT GuardianEmail FROM[ESCHOOL].[dbo].[Students] where GuardianPhone = '" + loginHistory.MemberID + "' or Email = '" + loginHistory.MemberID + "'").FirstOrDefault();

                        }
                        connection.Close();
                    }
                }

                if (loginHistory.LogType == "LogIn")
                {

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var IsExistLoginHistory = connection.Query<LoginHistory>("Select * from LoginHistory where  cast(InTime as date) = cast(GETDATE() as date) and MemberID = '" + loginHistory.MemberID + "'").FirstOrDefault();

                        connection.Close();
                        if (IsExistLoginHistory != null)
                        {
                            return -1;
                        }
                        else
                        {
                            connection.Open();
                            var affectedRows = connection.Execute("Insert into LoginHistory (MemberID,InTime," +
                                "LoginIP,SchoolId,IsApp,DeviceId,IsStudentOrParent) values (@MemberID, @InTime,@LoginIP,@SchoolId,@IsApp,@DeviceId,@IsStudentOrParent)",
                                new
                                {
                                    //Name = loginHistory.Name,
                                    MemberID = loginHistory.MemberID,
                                    //InTime = loginHistory.InTime,
                                    //OutTime = loginHistory.OutTime,
                                    InTime = DateTime.Now,
                                    //OutTime = null,
                                    //WorkingTimeHr = loginHistory.WorkingTimeHr,
                                    LoginIP = loginHistory.LoginIP,
                                    SchoolId = loginHistory.SchoolId,
                                    IsApp = loginHistory.IsApp,
                                    DeviceId = loginHistory.DeviceId,
                                    IsStudentOrParent = loginHistory.IsStudentOrParent
                                });
                            connection.Close();
                            return affectedRows;
                        }
                    }
                }
                else if (loginHistory.LogType == "LogOut")
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        var LID = new Int32();
                        LoginHistory findLastEntryByDeviceId = new LoginHistory();
                        try
                        {
                            LID = connection.Query<Int32>("Select max(LID) from LoginHistory where cast(InTime as date) = cast(GETDATE() as date) and MemberID = '" + loginHistory.MemberID + "'").FirstOrDefault();
                        }
                        catch (Exception)
                        {
                            return -2;
                        }
                        if (LID > 0)
                        {
                            findLastEntryByDeviceId = connection.Query<LoginHistory>("Select * from LoginHistory where LID='" + LID + "'").FirstOrDefault();
                        }

                        connection.Close();
                        connection.Open();
                        TimeSpan ts = (TimeSpan)(DateTime.Now - findLastEntryByDeviceId.InTime);
                        var affectedRows = connection.Execute("Update LoginHistory set OutTime = @OutTime, WorkingTimeHr=@WorkingTimeHr,DeviceId=@DeviceId Where LID = @LID",
                            new
                            {
                                LID = findLastEntryByDeviceId.LID,
                                MemberID = loginHistory.MemberID,
                                ////InTime = loginHistory.InTime,
                                ////OutTime = loginHistory.OutTime,
                                //InTime = DateTime.Now,
                                OutTime = DateTime.Now,
                                WorkingTimeHr = ts.ToString(@"hh\:mm\:ss"),
                                //LoginIP = loginHistory.LoginIP,
                                SchoolId = loginHistory.SchoolId,
                                //IsApp = loginHistory.IsApp,
                                DeviceId = loginHistory.DeviceId
                            });
                        connection.Close();
                        return affectedRows;
                    }
                }
                return 0;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public AuthenticateResponse GuardianAuthenticate(AuthenticateRequest model)
        {
            var user = GetGuardianCheck(model.Email, model.Password);

            //var user = _studentlogin.SingleOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }
        public VW_StudentLogin GetStudentCheck(string email, string password)
        {
            VW_StudentLogin studentProfileView = new VW_StudentLogin();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "";
            if (email.Contains('@'))
            {
                //query = "Select * FROM VW_StudentLogin WHERE Email='" + email + "' AND Password='" + password + "' AND LoginActive='1'";
                query = "SELECT dbo.Students.Email, dbo.Students.Password, dbo.Students.StudentID, dbo.Students.SchoolId, "+
                        "dbo.SubscriberSchools.SchoolName, dbo.Students.IsActive, dbo.Students.LoginActive, dbo.Students.SessionMasterID, "+
                        "dbo.Students.SessionDetailsID,dbo.Students.ClassId,Students.BearerToken,Students.BearerTokenExp " +
                        "FROM  dbo.Students "+
                        "INNER JOIN dbo.SubscriberSchools ON dbo.Students.SchoolId = dbo.SubscriberSchools.SchoolId " +
                        "inner join dbo.SessionMaster on SessionMaster.SessionID = Students.SessionMasterID " +
                        "inner join dbo.SessionDetails on SessionDetails.SessionDetailsID = Students.SessionDetailsID " +
                        "where Students.Email = '" + email + "' AND Students.Password = '" + password + "' AND Students.LoginActive = '1' " +
                        "and SessionMaster.IsActive = '1' and SubscriberSchools.IsActive = 1 and SessionMaster.IsActive = 1 and GETDATE()>= SessionDetails.SessionStartDate " +
                        "and GETDATE()<= SessionDetails.SessionEndDate";
            }
            else
            {
                //query = "Select * FROM VW_StudentLogin WHERE PhoneMobile='" + email + "' AND Password='" + password + "' AND LoginActive='1'";
                query = "SELECT dbo.Students.Email, dbo.Students.Password, dbo.Students.StudentID, dbo.Students.SchoolId, " +
                        "dbo.SubscriberSchools.SchoolName, dbo.Students.IsActive, dbo.Students.LoginActive, dbo.Students.SessionMasterID, " +
                        "dbo.Students.SessionDetailsID,dbo.Students.ClassId,Students.BearerToken,Students.BearerTokenExp " +
                        "FROM  dbo.Students " +
                        "INNER JOIN dbo.SubscriberSchools ON dbo.Students.SchoolId = dbo.SubscriberSchools.SchoolId " +
                        "inner join dbo.SessionMaster on SessionMaster.SessionID = Students.SessionMasterID " +
                        "inner join dbo.SessionDetails on SessionDetails.SessionDetailsID = Students.SessionDetailsID " +
                        "where Students.PhoneMobile = '" + email + "' AND Students.Password = '" + password + "' AND Students.LoginActive = '1' " +
                        "and SessionMaster.IsActive = '1' and SubscriberSchools.IsActive = 1 and SessionMaster.IsActive = 1 and GETDATE()>= SessionDetails.SessionStartDate " +
                        "and GETDATE()<= SessionDetails.SessionEndDate";
            }
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.StudentId = Convert.ToInt32(reader["StudentID"]);
                studentProfileView.Email = reader["Email"].ToString();
                studentProfileView.Password = reader["Password"].ToString();
                studentProfileView.SchoolId = reader["SchoolId"].ToString();
                studentProfileView.SchoolName = reader["SchoolName"].ToString();
                studentProfileView.LoginActive = Convert.ToInt32(reader["LoginActive"]);                
                studentProfileView.ClassId = reader["ClassId"].ToString();
                studentProfileView.BearerToken = reader["BearerToken"].ToString();
                if (!string.IsNullOrEmpty(studentProfileView.BearerToken))
                {
                    studentProfileView.BearerTokenExp = Convert.ToDateTime(reader["BearerTokenExp"]);
                }
                

                
                //studentProfileView.Token = reader["Token"].ToString();
            }
            reader.Close();
            connection.Close();
            if (studentProfileView.StudentId != 0)
            {
                return studentProfileView;
            }
            else
            {
                return null;
            }

        }

        #region "Guardian Check"
        public VW_StudentLogin GetGuardianCheck(string email, string password)
        {
            VW_StudentLogin studentProfileView = new VW_StudentLogin();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "";
            if (email.Contains('@'))
            {   
                //query = "Select * FROM Students S INNER JOIN SubscriberSchools Sc ON S.SchoolId = Sc.SchoolId WHERE GuardianEmail='" + email + "' AND GuardianPassword='" + password + "' AND LoginActive='1'";
                query = "Select * FROM Students S  INNER JOIN SubscriberSchools Sc ON S.SchoolId = Sc.SchoolId"+
                          " inner join SessionMaster sess on sess.SessionID = s.SessionMasterID"+
                          " WHERE GuardianEmail = '" + email + "' AND GuardianPassword = '" + password + "'"+
                          " AND LoginActive = '1' and sc.IsActive = 1 and GETDATE()<= SubscriptionExpireDate" +
                          " and sess.IsActive = '1'";

            }
            else
            {
                query = "Select * FROM Students S  INNER JOIN SubscriberSchools Sc ON S.SchoolId = Sc.SchoolId" +
                          " inner join SessionMaster sess on sess.SessionID = s.SessionMasterID" +
                          " WHERE GuardianPhone = '" + email + "' AND GuardianPassword = '" + password + "'" +
                          " AND LoginActive = '1' and sc.IsActive = 1 and GETDATE()<= SubscriptionExpireDate" +
                          " and sess.IsActive = '1'";
                //query = "Select * FROM Students S INNER JOIN SubscriberSchools Sc ON S.SchoolId = Sc.SchoolId WHERE GuardianPhone='" + email + "' AND GuardianPassword='" + password + "' AND LoginActive='1'";

            }
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.StudentId = Convert.ToInt32(reader["StudentID"]);
                studentProfileView.GuardianEmail = reader["GuardianEmail"].ToString();
                studentProfileView.GuardianPassword = reader["GuardianPassword"].ToString();
                studentProfileView.Email = reader["Email"].ToString();
                studentProfileView.Password = reader["Password"].ToString();
                studentProfileView.SchoolId = reader["SchoolId"].ToString();
                studentProfileView.SchoolName = reader["SchoolName"].ToString();
                studentProfileView.LoginActive = Convert.ToInt32(reader["LoginActive"]);
                studentProfileView.ClassId = reader["ClassId"].ToString();
                //studentProfileView.Token = reader["Token"].ToString();
            }
            reader.Close();
            connection.Close();
            if (studentProfileView.StudentId != 0)
            {
                return studentProfileView;
            }
            else
            {
                return null;
            }

        }
        #endregion
        public IEnumerable<VW_StudentLogin> GetAll()
        {
            List<VW_StudentLogin> studentProfileViews = new List<VW_StudentLogin>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM VW_StudentLogin";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                VW_StudentLogin studentProfileView = new VW_StudentLogin();
                studentProfileView.StudentId = Convert.ToInt32(reader["StudentId"]); ;
                studentProfileView.Email = reader["Email"].ToString();
                studentProfileView.Password = reader["Password"].ToString();
                //studentProfileView.Token = reader["Token"].ToString();
                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;
            //return _studentlogin;
        }

        public VW_StudentLogin GetById(int StudentloginId)
        {
            VW_StudentLogin studentProfileView = new VW_StudentLogin();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM VW_StudentLogin where StudentId=" + StudentloginId + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.StudentId = Convert.ToInt32(reader["StudentId"]); ;
                studentProfileView.Email = reader["Email"].ToString();
                studentProfileView.Password = reader["Password"].ToString();
                //studentProfileView.Token = reader["Token"].ToString();

            }
            return studentProfileView;
            //return _studentlogin.FirstOrDefault(x => x.StudentloginId == StudentloginId);
        }

        public async Task<bool> UpdateBearerTokenOnDB(string token, DateTime? tokenExp,int stdId)
        {
            string erroMsg = string.Empty;
            int rowCount = 0;
            string connectionString = _configuration.GetConnectionString("StudentDB");

            await using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "";
                    if (token!=null)
                    {
                        query = "UPDATE [dbo].[Students] SET BearerToken= @BearerToken,BearerTokenExp=@BearerTokenExp WHERE StudentID=@StudentID";
                    }
                    

                    SqlCommand cmd = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.Text,
                    };

                    cmd.Parameters.AddWithValue("@BearerToken", token);
                    cmd.Parameters.AddWithValue("@BearerTokenExp", tokenExp);
                    cmd.Parameters.AddWithValue("@StudentID", stdId);

                    con.Open();
                    rowCount = cmd.ExecuteNonQuery();

                    con.Close();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    erroMsg = ex.ToString();
                }
                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }

            if (rowCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateDeviceId(VW_StudentLogin entity)
        {
            string erroMsg = string.Empty;
            int rowCount = 0;
            string connectionString = _configuration.GetConnectionString("StudentDB");

            await using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "";
                    if (entity.Email.Contains('@'))
                    {
                        query = "UPDATE [dbo].[Students] SET DeviceId= @DeviceId WHERE StudentID=@StudentID AND Email = @Email";
                    }
                    else
                    {
                        query = "UPDATE [dbo].[Students] SET DeviceId= @DeviceId WHERE StudentID=@StudentID AND PhoneMobile = @Email";
                    }
                     
                    SqlCommand cmd = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.Text,
                    };

                    cmd.Parameters.AddWithValue("@DeviceId", entity.DeviceId);
                    cmd.Parameters.AddWithValue("@StudentID", entity.StudentId);
                    cmd.Parameters.AddWithValue("@Email", entity.Email);

                    con.Open();
                    rowCount = cmd.ExecuteNonQuery();

                    con.Close();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    erroMsg = ex.ToString();
                }
                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }

            if (rowCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> RemoveStdDeviceId(VW_StudentLogin entity)
        {
            string erroMsg = string.Empty;
            int rowCount = 0;
            string connectionString = _configuration.GetConnectionString("StudentDB");

            await using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    

                    string query = "";
                    if (entity.Email.Contains('@'))
                    {
                        query = "UPDATE [dbo].[Students] SET DeviceId= @DeviceId WHERE StudentID=@StudentID AND Email = @Email";
                    }
                    else
                    {
                        query = "UPDATE [dbo].[Students] SET DeviceId= @DeviceId WHERE StudentID=@StudentID AND PhoneMobile = @Email";
                    }
                    SqlCommand cmd = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.Text,
                    };


                    cmd.Parameters.AddWithValue("@DeviceId", string.Empty);
                    cmd.Parameters.AddWithValue("@StudentID", entity.StudentId);
                    cmd.Parameters.AddWithValue("@Email", entity.Email);


                    con.Open();
                    rowCount = cmd.ExecuteNonQuery();

                    con.Close();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    erroMsg = ex.ToString();
                }
                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }

            if (rowCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateGuardianDeviceId(VW_StudentLogin entity)
        {
            string erroMsg = string.Empty;
            int rowCount = 0;
            string connectionString = _configuration.GetConnectionString("StudentDB");

            await using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {

                    string query = "";
                    if (entity.GuardianEmail.Contains('@'))
                    {
                        query = "UPDATE [dbo].[Students] SET GuardianDeviceId= @GuardianDeviceId WHERE StudentID=@StudentID AND GuardianEmail = @GuardianEmail";
                    }
                    else
                    {
                        query = "UPDATE [dbo].[Students] SET GuardianDeviceId= @GuardianDeviceId WHERE StudentID=@StudentID AND GuardianPhone = @GuardianEmail";
                    }
                    SqlCommand cmd = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.Text,
                    };


                    cmd.Parameters.AddWithValue("@GuardianDeviceId", entity.GuardianDeviceId);
                    cmd.Parameters.AddWithValue("@StudentID", entity.StudentId);
                    cmd.Parameters.AddWithValue("@GuardianEmail", entity.GuardianEmail);


                    con.Open();
                    rowCount = cmd.ExecuteNonQuery();

                    con.Close();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    erroMsg = ex.ToString();
                }
                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }

            if (rowCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RemoveGuardianDeviceId(VW_StudentLogin entity)
        {
            string erroMsg = string.Empty;
            int rowCount = 0;
            string connectionString = _configuration.GetConnectionString("StudentDB");

            await using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    const string query = "UPDATE [dbo].[Students] SET GuardianDeviceId= @GuardianDeviceId WHERE StudentID=@StudentID AND GuardianEmail = @GuardianEmail";
                    SqlCommand cmd = new SqlCommand(query, con)
                    {
                        CommandType = CommandType.Text,
                    };

                    cmd.Parameters.AddWithValue("@GuardianDeviceId", string.Empty);
                    cmd.Parameters.AddWithValue("@StudentID", entity.StudentId);
                    cmd.Parameters.AddWithValue("@GuardianEmail", entity.Email);


                    con.Open();
                    rowCount = cmd.ExecuteNonQuery();

                    con.Close();
                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    erroMsg = ex.ToString();
                }
                finally
                {
                    if (con != null && con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }

            if (rowCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // helper methods
        private string generateJwtToken(VW_StudentLogin user)
        {
            // generate token that is valid for 365 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor();

            tokenDescriptor.Subject = new ClaimsIdentity(new[] { new Claim("StudentId", user.StudentId.ToString()) });
            tokenDescriptor.Expires = DateTime.UtcNow.AddDays(365);
            tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
