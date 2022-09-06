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
    public class PhotosServices:IPhotosServices
    {
        // Photos _ Photos = new  Photos(); //from Model
        List< Photos> _PhotosList = new List< Photos>(); //for resultset


        public List< Photos> Gets()
        {
            _PhotosList = new List< Photos>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query< Photos>(@"SELECT * FROM  Photos ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    _PhotosList = oStudents;
                }
                con.Close();
            }
            return _PhotosList;
        }


        List< Photos> IPhotosServices.Get(int pkId)
        {
            _PhotosList = new List< Photos>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query< Photos>(@"SELECT * FROM  Photos WHERE ClassId='" + pkId + "' ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    _PhotosList = oStudents;
                }
                con.Close();
            }
            return _PhotosList;
        }


    }
}

