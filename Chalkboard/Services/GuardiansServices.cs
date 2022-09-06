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
    public class GuardiansServices : IGuardiansServices
    {
        List<Guardians> _guardians = new List<Guardians>();

        public async Task<bool> Create(Guardians entity)
        {
            using IDbConnection oCon = new SqlConnection(Global.ConnectionsString);
            if (oCon.State == ConnectionState.Closed) oCon.Open();
            const string query = @"INSERT INTO Guardians( RelationId,StudentId,GuardianId,Relation,EntryBy,EntryDate,SchoolId) VALUES(@RelationId,@StudentId,@GuardianId,@Relation,@EntryBy,@EntryDate,@SchoolId)";

            try
            {
                await oCon.ExecuteAsync(query, new
                {
                    Id = Guid.NewGuid().ToString(),
                    entity.RelationId,
                    entity.StudentId,
                    entity.GuardianId,
                    entity.Relation,
                    entity.EntryBy,
                    entity.EntryDate,
                    entity.SchoolId
                    


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
            const string query = @"Delete Guardians where GuardiansId=@id";

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

        public List<Guardians> Get(int id)
        {
            _guardians = new List<Guardians>();

            using IDbConnection con = new SqlConnection(Global.ConnectionsString);
            if (con.State == ConnectionState.Closed) con.Open();
            var oGuardians = con.Query<Guardians>(@"SELECT * FROM Guardians Where GuardiansId ='" + id + "'").ToList();
            if (oGuardians.Any())
            {
                _guardians = oGuardians;
            }
            con.Close();
            return _guardians;
        }

        public List<Guardians> Gets()
        {
            _guardians = new List<Guardians>();
            using IDbConnection con = new SqlConnection(Global.ConnectionsString);
            if (con.State == ConnectionState.Closed) con.Open();
            var oGuardians = con.Query<Guardians>(@"SELECT * FROM Guardians").ToList();

            if (oGuardians.Any())
            {
                _guardians = oGuardians;
            }
            con.Close();
            return _guardians;
        }

        public async Task<bool> Update(int id, Guardians entity)
        {
            using IDbConnection oCon = new SqlConnection(Global.ConnectionsString);
            if (oCon.State == ConnectionState.Closed) oCon.Open();
            const string query = @"Update Guardians SET RelationId=@RelationId,StudentId=@StudentId,GuardianId=@GuardianId,Relation=@Relation,EntryBy=@EntryBy,EntryDate=@EntryDate,SchoolId=@SchoolId WHERE GuardiansId=@id";

            try
            {
                await oCon.ExecuteAsync(query, new
                {
                    entity.RelationId,
                    entity.StudentId,
                    entity.GuardianId,
                    entity.Relation,
                    entity.EntryBy,
                    entity.EntryDate,
                    entity.SchoolId,
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
