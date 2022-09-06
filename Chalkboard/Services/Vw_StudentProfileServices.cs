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
    public class Vw_StudentProfileServices : IVw_StudentProfileServices
    {
        List<Vw_StudentProfile> _Vw_StudentProfileList = new List<Vw_StudentProfile>(); //for resultset

        public List<Vw_StudentProfile> Get(int ClassId)
        {
            _Vw_StudentProfileList = new List<Vw_StudentProfile>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<Vw_StudentProfile>(@"SELECT * FROM Vw_StudentProfile WHERE sl='" + ClassId + "' ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    _Vw_StudentProfileList = oStudents;
                }
                con.Close();
            }
            return _Vw_StudentProfileList;
        }

        public List<Vw_StudentProfile> Gets()
        {
            _Vw_StudentProfileList = new List<Vw_StudentProfile>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<Vw_StudentProfile>(@"SELECT * FROM Vw_StudentProfile ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    _Vw_StudentProfileList = oStudents;
                }
                con.Close();
            }
            return _Vw_StudentProfileList;
        }
    }
}
