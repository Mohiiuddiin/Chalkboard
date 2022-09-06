//using ESCHOOL.IServices;
//using ESCHOOL.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Data;
//using System.Data.SqlClient;
//using ESCHOOL.Common;
//using Dapper;

//namespace ESCHOOL.Services
//{
//    public class vw_Bravo1Services : Ivw_BravoService
//    {
//        //vw_Bravo _vw_Bravo = new vw_Bravo(); //from Model
//        List<vw_Bravo> _vw_BravoList = new List<vw_Bravo>(); //for resultset


//        public List<vw_Bravo> Gets()
//        {
//            _vw_BravoList = new List<vw_Bravo>();

//            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
//            {
//                if (con.State == ConnectionState.Closed) con.Open();
//                var oStudents = con.Query<vw_Bravo>(@"SELECT StudentId, SubjectName, AVG(ResultMarks) AS ResultMarks
//                FROM vW_Bravo GROUP BY StudentId, SubjectName ").ToList();

//                if (oStudents != null && oStudents.Count() > 0)
//                {
//                    _vw_BravoList = oStudents;
//                }
//                con.Close();
//            }
//            return _vw_BravoList;
//        }


//        public List<vw_Bravo> Get(int studentId)
//        {
//            _vw_BravoList = new List<vw_Bravo>();

//            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
//            {
//                if (con.State == ConnectionState.Closed) con.Open();
//                var oStudents = con.Query<vw_Bravo>(@"SELECT StudentId, SubjectName, SubjectId, AVG(ResultMarks) AS ResultMarks
//                FROM vW_Bravo WHERE studentId='" + studentId + "' GROUP BY StudentId, SubjectId, SubjectName  ").ToList();

//                if (oStudents != null && oStudents.Count() > 0)
//                {
//                    _vw_BravoList = oStudents;
//                }
//                con.Close();
//            }
//            return _vw_BravoList;
//        }

//    }
//}

