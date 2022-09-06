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
using Microsoft.Extensions.Configuration;
using Chalkboard.Models.CustomModels;

namespace ESCHOOL.Services
{
    public class ExamResultServices : IExamResultServices
    {
        //ExamResult _ExamResult = new ExamResult(); //from Model
        List<ExamResult> _ExamResultList = new List<ExamResult>(); //for resultset

        private readonly IConfiguration _configuration;

        public ExamResultServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<ExamResult> Gets()
        {
            _ExamResultList = new List<ExamResult>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<ExamResult>(@"SELECT * FROM ExamResult ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    _ExamResultList = oStudents;
                }
                con.Close();
            }
            return _ExamResultList;
        }


        List<ExamResult> IExamResultServices.Get(int pkId)
        {
            _ExamResultList = new List<ExamResult>();

            using (IDbConnection con = new SqlConnection(Global.ConnectionsString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                var oStudents = con.Query<ExamResult>(@"SELECT * FROM ExamResult WHERE ClassId='" + pkId + "' ").ToList();

                if (oStudents != null && oStudents.Count() > 0)
                {
                    _ExamResultList = oStudents;
                }
                con.Close();
            }
            return _ExamResultList;
        }

        //public List<ExamResult> GetExamResult()
        //{
        //    string connectionString = _configuration.GetConnectionString("StudentDB");

        //    _ExamResultList = new List<ExamResult>();

        //    using (IDbConnection con = new SqlConnection(connectionString))
        //    {
        //        if (con.State == ConnectionState.Closed) con.Open();
        //        var oStudents = con.Query<ExamResult>(@"SELECT [ED].[ExamTitle] AS SubjectName,
        //             JSON_QUERY(( 
        //              SELECT  [ExamType], [ExamDate], [ResultDate], [ClassID], [SectionID], [SubjectId], [FullMarks], [GetMarks]
        //              FROM [dbo].[ExamResult] [ER], [dbo].[ExamSetup] [ES]
        //              WHERE [ER].[ExamDetailId] = [ED].[ExamDetailId] AND [ED].ExamTypeId = [ES].ExamSetupID
        //              FOR JSON PATH 
        //              )) AS [ExamResults]
        //            FROM [dbo].[ExamDetails] [ED]
        //            FOR 
        //             JSON PATH, WITHOUT_ARRAY_WRAPPER").ToList();

        //        if (oStudents != null && oStudents.Count() > 0)
        //        {
        //            _ExamResultList = oStudents;
        //        }
        //        con.Close();
        //    }
        //    return _ExamResultList;
        //}

        /// <summary>

        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// ///by old programmer
        //public string GetExamResult(int id)
        //{
        //    string connectionString = _configuration.GetConnectionString("StudentDB");

        //    var oStudents = "";

        //    using (IDbConnection con = new SqlConnection(connectionString))
        //    {
        //        if (con.State == ConnectionState.Closed) con.Open();
        //        oStudents = con.Query<string>(@"SELECT [S].[SubjectName] AS [SubjectName],
        //                             JSON_QUERY(( 
        //                              SELECT  [ExamType], [ExamDate], [ResultDate], [ER].[ClassID], [SectionID], [SubjectId], [SubjectName], [FullMarks], [GetMarks], [Remarks]
        //                              FROM [dbo].[ExamResult] [ER], [dbo].[ExamSetup] [ES], [dbo].[Subjects] [S]
        //                              WHERE [ER].[ExamDetailId] = [ED].[ExamDetailId] AND [ED].ExamTypeId = [ES].ExamSetupID 
        //                              AND [ER].ClassId = [ED].ClassID AND [ER].ClassId = [S].ClassID
        //                              AND [ER].SectionId = [ED].SectionId AND [ER].SchoolId = [ED].SchoolId
        //                              AND [ED].SubjectId = [S].SubjecId AND [ER].[StudentId] = '" + id + "' " +
        //                                    "FOR JSON PATH )) AS [ResultDetails] " +
        //                                    "FROM [dbo].[ExamDetails] [ED] INNER JOIN [dbo].[ExamResult] [ER] ON [ER].[ExamDetailId] = [ED].[ExamDetailId] " +
        //                                    "INNER JOIN [dbo].[Subjects] [S] ON [ED].SubjectId = [S].SubjecId " +
        //                                    "WHERE [ER].[StudentId] = '" + id + "' " +
        //                                "FOR JSON PATH ").FirstOrDefault();


        //        //oStudents = con.Query<string>(@"SELECT [ED].[ExamTitle] AS ExamTitle,
        //        //             JSON_QUERY(( 
        //        //              SELECT  [ExamType], [ExamDate], [ResultDate], [ER].[ClassID], [SectionID], [SubjectId], [SubjectName], [FullMarks], [GetMarks], [Remarks]
        //        //              FROM [dbo].[ExamResult] [ER], [dbo].[ExamSetup] [ES], [dbo].[Subjects] [S]
        //        //              WHERE [ER].[ExamDetailId] = [ED].[ExamDetailId] AND [ED].ExamTypeId = [ES].ExamSetupID 
        //        //              AND [ER].ClassId = [ED].ClassID AND [ER].ClassId = [S].ClassID
        //        //              AND [ER].SectionId = [ED].SectionId AND [ER].SchoolId = [ED].SchoolId
        //        //              AND [ED].SubjectId = [S].SubjecId
        //        //              FOR JSON PATH 
        //        //              )) AS [ExamResults]
        //        //                FROM [dbo].[ExamDetails] [ED] INNER JOIN [dbo].[ExamResult] [ER] ON [ER].[ExamDetailId] = [ED].[ExamDetailId] 
        //        //                WHERE [ER].[StudentId] = '" + id + "' " +
        //        //                "FOR JSON PATH ").FirstOrDefault();

        //        con.Close();
        //    }
        //    return oStudents;
        //}

        //crated By Mohiuddin
        public Data GetExamResult(int id)
        {
            List<ExamResultsMh> vm = new List<ExamResultsMh>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = @"select exd.ExamDate Date,
                exd.ExamTitle ExamTite, ex.FullMarks,ex.GetMarks,ex.Remarks,exs.ExamType ExamType,
                 s.SubjectName,t.TeacherName,std.StudentNameE,scl.SchoolName,ex.ResultDate,Stat = 'Exam',
                scl.SchoolId,c.sl ClassId, s.SubjecId,sc.SectionId,std.StudentID,t.TeacherId,TaskId = 0,exs.ExamSetupID,TestExamResultId = 0,
                exd.ExamDetailId,exd.ExamTypeId,exd.ExamSyllabus,exd.ExamVenue,exd.ExamStart,exd.ExamEnd,exd.EntryBy,
                exd.EntryDate,exd.Updatedby,ISNULL(exd.UpdatedDate,'')UpdatedDate,exd.IsActive
                ,TestExamResultPublished = 0,ex.GPA,ex.ExamSession,TaskDetails = '',TaskNumber='',TaskTypeName=''
                into #temp1 from ExamDetails exd 
                inner join SubscriberSchools scl on scl.SchoolId = exd.SchoolId inner join Class c on c.sl = exd.ClassID and c.SchoolId = scl.SchoolId
                inner join Sections sc on sc.SectionId = exd.SectionID and sc.ClassId = c.sl inner join Subjects s on s.SubjecId = exd.SubjectId and s.ClassId = c.sl
                inner join Teachers t on t.TeacherId = sc.TeacherID inner join ExamResult ex on ex.ExamDetailId = exd.ExamDetailId
                inner join ExamSetup exs on exs.ExamSetupID = exd.ExamTypeId inner join Students std on std.StudentID = ex.StudentId
                --where std.StudentID = 202217
                select ter.TaskDate Date, ter.TaskNumber ExamTitle, ter.FullMarks,--ex.TaskNumber,
                ex.GetMarks,ex.Remarks,exs.ExamType ExamType,--,ter.TaskMarked GetMarks,
                s.SubjectName,t.TeacherName,std.StudentNameE,scl.SchoolName,ex.ResultDate,Stat = 'Test',
                scl.SchoolId,c.sl ClassId, s.SubjecId,sc.SectionId,std.StudentID,t.TeacherId,ter.TaskId,exs.ExamSetupID,ex.TestExamResultId
                ,ExamDetailId = 0,ExamTypeId = 0,ExamSyllabus = '',ExamVenue = '',ExamStart = '',ExamEnd = '',EntryBy = '',
                EntryDate = '',Updatedby = '',UpdatedDate = '',IsActive = ''
                ,ex.TestExamResultPublished,ex.GPA,ex.ExamSession,ter.TaskDetails,ter.TaskNumber,tsktype.TaskTypeName--,ex.ResultDate
                into #temp2 
                from Tasks ter
                inner
                join SubscriberSchools scl on scl.SchoolId = ter.SchoolId
                inner
                join Class c on c.sl = ter.ClassID and c.SchoolId = scl.SchoolId
                inner join Sections sc on sc.SectionId = ter.SectionID and sc.ClassId = c.sl
                inner join Subjects s on s.SubjecId = ter.SubjectId and s.ClassId = c.sl
                inner join Teachers t on t.TeacherId = sc.TeacherID
                inner join TestExamResult ex on ex.TaskId = ter.TaskId-- and ex.ClassId = c.sl and ex.SchoolId = s.SchoolId and ex.SectionId = sc.SectionId
                inner join ExamSetup exs on exs.ExamSetupID = ter.ExamTypeId
                inner join Students std on std.StudentID = ex.StudentId 
                inner join TaskTypes tsktype on ter.TaskTypeId = tsktype.TaskTypeId
                --inner join Tasks tsk on tsk.ExamTypeId = exs.ExamSetupID

                select* into #final from #temp1 union select * from #temp2 
                select* from #final where StudentID=" + id +
                "drop table #temp1,#temp2,#final";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                ExamResultsMh model = new ExamResultsMh();
                model.Date = Convert.ToDateTime(reader["Date"]);
                model.ExamTitle = reader["ExamTite"].ToString();
                model.GPA = Convert.ToDouble(reader["GPA"].ToString());
                model.FullMarks = Convert.ToInt32(reader["FullMarks"].ToString());//FullMarks
                model.GetMarks = reader["GetMarks"].ToString();
                model.Remarks = reader["Remarks"].ToString();
                model.ExamType = reader["ExamType"].ToString();
                model.ExamSession = Convert.ToDateTime(reader["ExamSession"]);
                model.TaskDetails = reader["TaskDetails"].ToString();
                model.SubjectName = reader["SubjectName"].ToString();
                model.TeacherName = reader["TeacherName"].ToString();
                model.StudentNameE = reader["StudentNameE"].ToString();
                model.SchoolName = reader["SchoolName"].ToString();
                model.ResultDate = Convert.ToDateTime(reader["ResultDate"]);
                model.Stat = reader["Stat"].ToString();
                model.SchoolId = Convert.ToInt32(reader["SchoolId"].ToString());
                model.ClassId = Convert.ToInt32(reader["ClassId"].ToString());
                model.SubjectId = Convert.ToInt32(reader["SubjecId"].ToString());
                model.SectionId = Convert.ToInt32(reader["SectionId"].ToString());
                model.StudentId = Convert.ToInt32(reader["StudentID"].ToString());
                model.TeacherId = Convert.ToInt32(reader["TeacherId"].ToString());
                model.TaskId = Convert.ToInt32(reader["TaskId"].ToString());
                model.ExamSetupID = Convert.ToInt32(reader["ExamSetupID"].ToString());
                model.TestExamResultId = Convert.ToInt32(reader["TestExamResultId"].ToString());
                model.ExamDetailId = Convert.ToInt32(reader["ExamDetailId"].ToString());
                model.ExamTypeId = Convert.ToInt32(reader["ExamTypeId"].ToString());
                model.ExamSyllabus = reader["ExamSyllabus"].ToString();
                model.ExamVenue = reader["ExamVenue"].ToString();
                model.ExamStart = reader["ExamStart"].ToString();
                model.ExamEnd = reader["ExamEnd"].ToString();
                model.EntryBy = reader["EntryBy"].ToString();
                model.EntryDate = Convert.ToDateTime(reader["EntryDate"]);
                model.Updatedby = reader["Updatedby"].ToString();
                model.TestExamResultPublished = Convert.ToInt32(reader["TestExamResultPublished"].ToString());
                model.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                model.IsActive = Convert.ToInt32(reader["IsActive"]);
                model.TaskNumber = reader["TaskNumber"].ToString();
                model.TaskTypeName = reader["TaskTypeName"].ToString();

                vm.Add(model);
            }
            reader.Close();
            connection.Close();

            List<ExamResultsReturnJsonMH> exams = new List<ExamResultsReturnJsonMH>();
            List<ExamResultsMh> x1 = new List<ExamResultsMh>();



            var examlistsQuery = from x in vm.ToList()
                                 group x by new { x.SubjectId, x.SubjectName, x.ExamSetupID, x.ExamType } into egroup
                                 select new
                                 {
                                     egroup.Key.ExamSetupID,
                                     egroup.Key.SubjectName,
                                     egroup.Key.SubjectId,
                                     egroup.Key.ExamType,
                                     //egroup.Key.Stat,

                                 };
            var examlists = examlistsQuery.ToList();
            var terms = from x in vm.ToList()
                        group x by new { x.ExamSetupID, x.ExamType } into egroup
                        select new
                        {
                            egroup.Key.ExamSetupID,
                            egroup.Key.ExamType
                        };

            foreach (var term in terms)
            {

                ExamResultsReturnJsonMH exam = new ExamResultsReturnJsonMH();
                exam.ExamSetupID = term.ExamSetupID;
                exam.ExamType = term.ExamType;
                exam.Subjects = new Subjects();
                Subjects MainSubject = new Subjects();
                MainSubject.Results = new List<Results>();
                foreach (var item in examlists)
                {
                    //
                    List<Results> Subject = new List<Results>();
                    //Results s = new Results();
                    //exam.Subjects = new List<Subjects>();
                    x1 = vm.Where(x => (x.ExamSetupID == item.ExamSetupID && x.SubjectId == item.SubjectId)).ToList();
                    //exam.Subjects = new Subjects();

                    //exam.Subjects.Results = new List<Results>();
                    Results subject1 = new Results();
                    subject1.SubjectId = item.SubjectId;
                    subject1.SubjectName = item.SubjectName;
                    subject1.TestDetails = new List<TestDetails>();
                    foreach (var item1 in x1)
                    {
                        int count = 0;
                        if (item1.Stat == "Exam")
                        {
                            subject1.ExamDetails = new Chalkboard.Models.CustomModels.ExamDetails()
                            {
                                ClassID = item1.ClassId,
                                SubjectId = item1.SubjectId,
                                ExamDetailId = item1.ExamDetailId,
                                ExamDate = item1.Date,
                                ExamEnd = item1.ExamEnd,
                                ExamStart = item1.ExamStart,
                                ExamSyllabus = item1.ExamSyllabus,
                                ExamTitle = item1.ExamTitle,
                                ExamTypeId = item1.ExamTypeId,
                                ExamVenue = item1.ExamVenue,
                                FullMarks = item1.FullMarks,
                                GetMarks = item1.GetMarks,
                                IsActive = item1.IsActive,
                                Marks = item1.Marks,
                                SchoolId = item1.SchoolId,
                                SectionID = item1.SectionId,
                                Remarks = item1.Remarks
                                
                            };
                            count++;
                        }
                        else if (item1.Stat == "Test")
                        {
                            //subjects.SubjectId = item1.SubjectId;
                            //subjects.SubjectName = item1.SubjectName;
                            //exam.ExamSetupID = item.ExamSetupID;
                            //exam.ExamType = item.ExamType;
                            //exam.Subjects = new List<Subjects>();
                            //foreach (var item1 in x1)
                            //{

                            //    //exam.Subjects.Add(subjects);
                            //}
                            subject1.TestDetails.Add(new Chalkboard.Models.CustomModels.TestDetails()
                            {
                                TaskId = item1.TaskId,
                                SubjectId = item1.SubjectId,
                                SectionId = item1.SectionId,
                                SubjectName = item1.SubjectName,
                                TaskDetails = item1.TaskDetails,
                                TaskTypeName = item1.TaskTypeName,
                                TeacherName = item1.TeacherName,
                                TestExamResults = new Chalkboard.Models.CustomModels.TestExamResults()
                                {
                                    ExamSession = item1.ExamSession,
                                    FullMarks = item1.FullMarks,
                                    GetMarks = item1.GetMarks,
                                    GPA = item1.GPA,
                                    Remarks = item1.Remarks,
                                    ResultDate = item1.ResultDate,
                                    StudentId = item1.StudentId,
                                    TaskNumber = item1.TaskNumber,
                                    TestExamResultId = item1.TestExamResultId,
                                    TestExamResultPublished = item1.TestExamResultPublished,
                                }

                            });
                            count++;
                        }

                    }

                    Subject.Add(subject1);

                    //Subject.Add(subject1);
                    //
                    MainSubject.Results.Add(subject1);

                }
                exam.Subjects.Results = MainSubject.Results;

                query = "select ex.SubjecId,ex.SubjectName from ("+
                " SELECT s.SubjecId,s.SubjectName,e.SubjectId status1,t.SubjectName status2"+
                 " FROM [ESCHOOL].[dbo].[Subjects] s"+
                 " left join ExamDetails e on e.SubjectId=s.SubjecId "+
                 " left join Task t on t.SubjectName=s.SubjectName"+
                  " where s.SchoolId = " + vm[0].SchoolId + " and s.ClassId =" + vm[0].ClassId +
                ") as ex where ex.status1 is null and ex.status2 is null";
                //              query = @"SELECT SubjecId,SubjectName
                //FROM[ESCHOOL].[dbo].[Subjects] where SchoolId = "+vm[0].SchoolId+" and ClassId = 2110" + vm[0].ClassId;
                SqlCommand com1 = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader1 = com1.ExecuteReader();

                while (reader1.Read())
                {
                    exam.Subjects.Results.Add(new Results
                    {
                        SubjectId = Convert.ToInt32(reader1["SubjecId"]),
                        SubjectName = reader1["SubjectName"].ToString(),
                        ExamDetails = null,
                        TestDetails = null
                    });
                }
                reader.Close();
                connection.Close();
                exams.Add(exam);
            }
            Data data = new Data();
            data.JsonData = new List<ExamResultsReturnJsonMH>();

            data.JsonData = exams.ToList();

            return data;
        }


    }
}

