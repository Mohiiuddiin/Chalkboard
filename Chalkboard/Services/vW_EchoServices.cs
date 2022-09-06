using ChalkboardAPI.Models;
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
    public class vW_EchoServices : IvW_EchoServices
    {
        List<vW_Echo> __vW_EchoList = new List<vW_Echo>();
        //public List<vW_Echo> Get(int id,int SubjectId)
        //{
           
        //}

        //public List<vW_Echo> Get(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public List<vW_Echo> Get(int id)
        {
            __vW_EchoList = new List<vW_Echo>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<vW_Echo>(@"SELECT * FROM vW_Echo WHERE ClassId ='" + id + "' ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    __vW_EchoList = oStudents;
                }
                con.Close();
            }
            return __vW_EchoList;
        }

        //public List<vW_Echo> Gets(int taskId)
        //{

        //}

        public List<vW_Echo> Gets()
        {
            __vW_EchoList = new List<vW_Echo>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<vW_Echo>(@"SELECT * FROM vW_Echo ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    __vW_EchoList = oStudents;
                }
                con.Close();
            }
            return __vW_EchoList;
        }
    }
}
