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
    public class StudentprofileServices : IStudentprofileServices
    {
        List<Studentprofile> _studentprofile = new List<Studentprofile>();

        public async Task<bool> Create(Studentprofile entity)
        {
            using IDbConnection oCon = new SqlConnection(Global.ConnectionsString);
            if (oCon.State == ConnectionState.Closed) oCon.Open();
            const string query = @"INSERT INTO Studentprofile( StudentName,StudentID,Standard,GuardianName,DOB,BloodGroup,MobileNo,Division) VALUES(@StudentName,@StudentID,@Standard,@GuardianName,@DOB,@BloodGroup,@MobileNo,@Division)";

            try
            {
                await oCon.ExecuteAsync(query, new
                {
                    Id = Guid.NewGuid().ToString(),
                    entity.StudentName,
                    entity.StudentID,
                    entity.Standard,
                    entity.GuardianName,
                    entity.DOB,
                    entity.BloodGroup,
                    entity.MobileNo,
                    entity.Division


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
            const string query = @"Delete Studentprofile where StudentprofileId=@id";

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

        public List<Studentprofile> Get(int id)
        {
            _studentprofile = new List<Studentprofile>();

            using IDbConnection con = new SqlConnection(Global.ConnectionsString);
            if (con.State == ConnectionState.Closed) con.Open();
            var oStudentprofile = con.Query<Studentprofile>(@"SELECT * FROM Studentprofile Where StudentID ='" + id + "'").ToList();
            if (oStudentprofile.Any())
            {
                _studentprofile = oStudentprofile;
            }
            con.Close();
            return _studentprofile;
        }

        public List<Studentprofile> Gets()
        {

            _studentprofile = new List<Studentprofile>();
            using IDbConnection con = new SqlConnection(Global.ConnectionsString);
            if (con.State == ConnectionState.Closed) con.Open();
            var oStudentprofile = con.Query<Studentprofile>(@"SELECT * FROM Studentprofile").ToList();

            if (oStudentprofile.Any())
            {
                _studentprofile = oStudentprofile;
            }
            con.Close();
            return _studentprofile;
        }

        public async Task<bool> Update(int id, Studentprofile entity)
        {
            using IDbConnection oCon = new SqlConnection(Global.ConnectionsString);
            if (oCon.State == ConnectionState.Closed) oCon.Open();
            const string query = @"Update Studentprofile SET StudentName=@StudentName,StudentID=@StudentID,Standard=@Standard,GuardianName=@GuardianName,DOB=@DOB,BloodGroup=@BloodGroup,MobileNo=@MobileNo,Division=@Division WHERE StudentprofileId=@id";

            try
            {
                await oCon.ExecuteAsync(query, new
                {
                    entity.StudentName,
                    entity.StudentID,
                    entity.Standard,
                    entity.GuardianName,
                    entity.DOB,
                    entity.BloodGroup,
                    entity.MobileNo,
                    entity.Division,
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
