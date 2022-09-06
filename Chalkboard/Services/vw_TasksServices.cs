using ChalkboardAPI.IServices;
using Dapper;
using ESCHOOL.Common;
using ESCHOOL.IServices;
using ChalkboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ESCHOOL.Models;
using ChalkboardAPI.Models.CustomModels;

namespace ChalkboardAPI.Services
{
    public class vw_TasksServices : Ivw_TasksServices
    {
        List<vw_Tasks> _vw_TasksList = new List<vw_Tasks>(); //for resultset
        private readonly IConfiguration _configuration;

        public vw_TasksServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<vw_Tasks> Get(int Id)
        {
            _vw_TasksList = new List<vw_Tasks>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<vw_Tasks>(@"SELECT * FROM vw_Tasks WHERE TaskId='" + Id + "' ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    _vw_TasksList = oStudents;
                }
                con.Close();
            }
            return _vw_TasksList;
        }

        public List<Tasks> GetTaskListBySubjectId(int Id)
        {

            List<Tasks> studentProfileViews = new List<Tasks>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select ts.*,th.TeacherName, s.SectionName, tt.TaskTypeName from Tasks ts Left Join Teachers th on th.TeacherId=ts.TeacherId Left Join Sections s on s.SectionId=ts.SectionId Left Join TaskTypes tt on tt.TaskTypeId=ts.TaskTypeId where ts.SubjectId=" + Id + "order by ts.EntryDate desc";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Tasks studentProfileView = new Tasks();
                studentProfileView.TaskId = Convert.ToInt32(reader["TaskId"]);
                studentProfileView.SubjectId = Convert.ToInt32(reader["SubjectId"]);
                studentProfileView.SectionId = Convert.ToInt32(reader["SectionId"]);
                studentProfileView.TaskDate = Convert.ToDateTime(reader["TaskDate"]);
                //studentProfileView.TaskHeadline = reader["TaskHeadline"].ToString();
                studentProfileView.TaskDetails = reader["TaskDetails"].ToString();
                studentProfileView.TeacherName = reader["TeacherName"].ToString();
                studentProfileView.SectionName = reader["SectionName"].ToString();
                studentProfileView.TaskTypeName = reader["TaskTypeName"].ToString();
                studentProfileView.EntryBy = reader["EntryBy"].ToString();

                studentProfileView.EntryDate = Convert.ToDateTime(reader["EntryDate"]);

                //studentProfileView.Name = reader["Name"].ToString();

                studentProfileViews.Add(studentProfileView);

            }
            reader.Close();
            connection.Close();
            return studentProfileViews;



            //_vw_TasksList = new List<vw_Tasks>();

            //using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            //{
            //    if (con.State == ConnectionState.Closed) con.Open();
            //    var oStudents = con.Query<vw_Tasks>(@"Select * from vw_Tasks where SubjecId=='" + Id + "' ").ToList();

            //    if (oStudents != null && oStudents.Count() > 0)
            //    {
            //        _vw_TasksList = oStudents;
            //    }
            //    con.Close();
            //}
            //return _vw_TasksList;
        }
        public List<Subjects> GetSubjectsByStudentId(string id)
        {
            
            List<Subjects> studentProfileViews = new List<Subjects>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select s.SubjecId,s.SubjectName from Subjects s left join Classes c on c.ClassId = s.ClassId " +
                "left join Students std on std.ClassId = c.ClassId where std.StudentID='" + id + "' " +
                " and EXISTS (SELECT * FROM Tasks WHERE SubjectId = s.SubjecId) ";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                Subjects studentProfileView = new Subjects();
                studentProfileView.SubjecId = Convert.ToInt32(reader["SubjecId"]);
                studentProfileView.SubjectName = reader["SubjectName"].ToString();

                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;




            //List<Subjects> studentProfileViews = new List<Subjects>();

            //using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            //{
            //    if (con.State == ConnectionState.Closed) con.Open();
            //    var oStudents = con.Query<Subjects>(@"Select s.SubjecId,s.SubjectName from Subjects s left join Classes c on c.ClassId = s.ClassId left join Students std on std.ClassId = c.ClassId where std.StudentID='" + id + "'").ToList();

            //    if (oStudents != null && oStudents.Count() > 0)
            //    {
            //        studentProfileViews = oStudents;
            //    }
            //    con.Close();
            //}

            //return studentProfileViews;
        }

        public List<ClassTestResult> GetSubjectsTestResultByStudentId(string id)
        {
            List<ClassTestResult> studentProfileViews = new List<ClassTestResult>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = @"Select Subjects.SubjecId,Subjects.SubjectName, 
                                iif(((Select SUM(ClassTestResultDetail.MarkInPercent) from ClassTestResultDetail 
                                LEFT JOIN ClassTestResult ON ClassTestResult.ClassTestResultId=ClassTestResultDetail.ClassTestResultId
                                LEFT JOIN Students ON Students.sl=ClassTestResultDetail.StudentId
                                Where ClassTestResult.SubjectId=Subjects.SubjecId AND Students.StudentId='" + id + @"')/ 
                                 (Select count(ClassTestResultDetail.Id) from ClassTestResultDetail 
                                LEFT JOIN ClassTestResult ON ClassTestResult.ClassTestResultId=ClassTestResultDetail.ClassTestResultId
                                LEFT JOIN Students ON Students.sl=ClassTestResultDetail.StudentId
                                Where ClassTestResult.SubjectId=Subjects.SubjecId AND Students.StudentId='" + id + @"')) IS NOT NULL,
                                Convert(nvarchar(100), 
                                ((Select SUM(ClassTestResultDetail.MarkInPercent) from ClassTestResultDetail 
                                LEFT JOIN ClassTestResult ON ClassTestResult.ClassTestResultId=ClassTestResultDetail.ClassTestResultId
                                LEFT JOIN Students ON Students.sl=ClassTestResultDetail.StudentId
                                Where ClassTestResult.SubjectId=Subjects.SubjecId AND Students.StudentId='" + id + @"')/ 
                                 (Select count(ClassTestResultDetail.Id) from ClassTestResultDetail 
                                LEFT JOIN ClassTestResult ON ClassTestResult.ClassTestResultId=ClassTestResultDetail.ClassTestResultId
                                LEFT JOIN Students ON Students.sl=ClassTestResultDetail.StudentId
                                Where ClassTestResult.SubjectId=Subjects.SubjecId AND Students.StudentId='" + id + @"'))),
                                'N/A') as [AverageMark] 
                                from Subjects
                                LEFT JOIN Classes ON Classes.ClassId=Subjects.ClassId
                                LEFT JOIN Students ON Students.ClassId=Classes.ClassId
                                Where Students.StudentID='" + id + @"'";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                ClassTestResult studentProfileView = new ClassTestResult();
                studentProfileView.SubjectName = reader["SubjectName"].ToString();
                studentProfileView.AverageMark = reader["AverageMark"].ToString();
                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;
        }
        public List<vw_Tasks> Gets()
        {
            _vw_TasksList = new List<vw_Tasks>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<vw_Tasks>(@"SELECT * FROM vw_Tasks ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    _vw_TasksList = oStudents;
                }
                con.Close();
            }
            return _vw_TasksList;
        }

    }
}
