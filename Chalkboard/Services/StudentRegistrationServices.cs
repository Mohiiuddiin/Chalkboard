using Dapper;
using ESCHOOL.Common;
using ESCHOOL.IServices;
using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Services
{
    public class StudentRegistrationServices : IStudentRegistrationServices
    {
        List<StudentRegistration> _studentRegistration = new List<StudentRegistration>();

        public async Task<bool> Create(StudentRegistration entity)
        {
            using IDbConnection oCon = new SqlConnection(Global.ConnectionsString);
            if (oCon.State == ConnectionState.Closed) oCon.Open();
            const string query = @"INSERT INTO StudentRegistration( Email,Password) VALUES(@Email,@Password)";

            try
            {
                await oCon.ExecuteAsync(query, new
                {
                    Id = Guid.NewGuid().ToString(),
                    entity.Email,
                    entity.Password

                }, commandType: CommandType.Text);
            }
            finally
            {
                oCon.Close();
            }

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            using IDbConnection con = new SqlConnection(Global.ConnectionsString);
            if (con.State == ConnectionState.Closed) con.Open();
            const string query = @"Delete StudentRegistration where StudentRegistrationId=@id";

            try
            {
                await con.ExecuteAsync(query, new { id }, commandType: CommandType.Text);
            }
            finally
            {
                con.Close();
            }

            return true;
        }

        public List<StudentRegistration> Get(int id)
        {
            _studentRegistration = new List<StudentRegistration>();

            using IDbConnection con = new SqlConnection(Global.ConnectionsString);
            if (con.State == ConnectionState.Closed) con.Open();
            var oStudentRegistration = con.Query<StudentRegistration>(@"SELECT * FROM StudentRegistration Where StudentRegistrationId ='" + id + "'").ToList();
            if (oStudentRegistration.Any())
            {
                _studentRegistration = oStudentRegistration;
            }
            con.Close();
            return _studentRegistration;
        }

        public List<StudentRegistration> Gets()
        {
            _studentRegistration = new List<StudentRegistration>();
            using IDbConnection con = new SqlConnection(Global.ConnectionsString);
            if (con.State == ConnectionState.Closed) con.Open();
            var oStudentRegistration = con.Query<StudentRegistration>(@"SELECT * FROM StudentRegistration").ToList();

            if (oStudentRegistration.Any())
            {
                _studentRegistration = oStudentRegistration;
            }
            con.Close();
            return _studentRegistration;
        }

        public async Task<bool> Update(int id, StudentRegistration entity)
        {
            using IDbConnection oCon = new SqlConnection(Global.ConnectionsString);
            if (oCon.State == ConnectionState.Closed) oCon.Open();
            const string query = @"Update StudentRegistration SET Email=@Email,Password=@Password WHERE StudentRegistrationId=@id";

            try
            {
                await oCon.ExecuteAsync(query, new
                {
                    entity.Email,
                    entity.Password,
                    id

                }, commandType: CommandType.Text);
            }
            finally
            {
                oCon.Close();
            }

            return true;
        }
    }
}
