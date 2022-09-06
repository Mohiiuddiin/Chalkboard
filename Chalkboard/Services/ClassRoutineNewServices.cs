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
    public class ClassRoutineNewServices : IClassRoutineNewServices
    {
        List<ClassRoutineNew> _classRoutineNew = new List<ClassRoutineNew>();

        public async Task<bool> Create(ClassRoutineNew entity)
        {
            using IDbConnection oCon = new SqlConnection(Global.ConnectionsString);
            if (oCon.State == ConnectionState.Closed) oCon.Open();
            const string query = @"INSERT INTO ClassRoutineNew( ClassesId,SectionId,SubjectId,TeacherId,Day,StartTime,EndTime,EntryDate,EntryBy) VALUES(@ClassesId,@SectionId,@SubjectId,@TeacherId,@Day,@StartTime,@EndTime,@EntryDate,@EntryBy)";

            try
            {
                await oCon.ExecuteAsync(query, new
                {
                    Id = Guid.NewGuid().ToString(),
                    entity.ClassesId,
                    entity.SectionId,
                    entity.SubjectId,
                    entity.TeacherId,
                    entity.Day,
                    entity.StartTime,
                    entity.EndTime,
                    entity.EntryDate,
                    entity.EntryBy


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
            const string query = @"Delete ClassRoutineNew where ClassRoutineId=@id";

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

        public List<ClassRoutineNew> Get(int id)
        {
            _classRoutineNew = new List<ClassRoutineNew>();

            using IDbConnection con = new SqlConnection(Global.ConnectionsString);
            if (con.State == ConnectionState.Closed) con.Open();
            var oClassRoutineNew = con.Query<ClassRoutineNew>(@"SELECT * FROM ClassRoutineNew Where ClassRoutineId ='" + id + "'").ToList();
            if (oClassRoutineNew.Any())
            {
                _classRoutineNew = oClassRoutineNew;
            }
            con.Close();
            return _classRoutineNew;
        }

        public List<ClassRoutineNew> Gets()
        {
            _classRoutineNew = new List<ClassRoutineNew>();
            using IDbConnection con = new SqlConnection(Global.ConnectionsString);
            if (con.State == ConnectionState.Closed) con.Open();
            var oClassRoutineNew = con.Query<ClassRoutineNew>(@"SELECT * FROM ClassRoutineNew").ToList();

            if (oClassRoutineNew.Any())
            {
                _classRoutineNew = oClassRoutineNew;
            }
            con.Close();
            return _classRoutineNew;
        }

        public async Task<bool> Update(int id, ClassRoutineNew entity)
        {
            using IDbConnection oCon = new SqlConnection(Global.ConnectionsString);
            if (oCon.State == ConnectionState.Closed) oCon.Open();
            const string query = @"Update ClassRoutineNew SET ClassesId=@ClassesId,SectionId=@SectionId,SubjectId=@SubjectId,TeacherId=@TeacherId,Day=@Day,StartTime=@StartTime,EndTime=@EndTime,EntryDate=@EntryDate,EntryBy=@EntryBy WHERE ClassRoutineId=@id";

            try
            {
                await oCon.ExecuteAsync(query, new
                {
                    entity.ClassesId,
                    entity.SectionId,
                    entity.SubjectId,
                    entity.TeacherId,
                    entity.Day,
                    entity.StartTime,
                    entity.EndTime,
                    entity.EntryDate,
                    entity.EntryBy,
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
