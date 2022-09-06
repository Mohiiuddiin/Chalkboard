using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ESCHOOL.Common;
using Dapper;
using ESCHOOL.IServices;
using ESCHOOL.Models;




namespace ESCHOOL.Services
{
    public class GuardianServices:IGuardianServices
    {
        //Guardian _Guardian = new Guardian(); //from Model
        List<Guardian> _GuardianList = new List<Guardian>(); //for resultset


        public List<Guardian> Gets()
        {
            _GuardianList = new List<Guardian>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<Guardian>(@"SELECT * FROM Guardian ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    _GuardianList = oStudents;
                }
                con.Close();
            }
            return _GuardianList;
        }


        List<Guardian> IGuardianServices.Get(int pkId)
        {
            _GuardianList = new List<Guardian>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<Guardian>(@"SELECT * FROM Guardian WHERE ClassId='" + pkId + "' ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    _GuardianList = oStudents;
                }
                con.Close();
            }
            return _GuardianList;
        }


    }
}


 
