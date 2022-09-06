using ChalkboardAPI.Helpers;
using ChalkboardAPI.Models;
using ChalkboardAPI.Models.CustomModels;
using Dapper;
using ESCHOOL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ESCHOOL.Services
{
    public interface IExamDetailsServices
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<ExamDetails> GetAll();
        ExamDetails GetById(int id);
        string GetExamDetails(int id);
        List<ExamSetup> GetExamType(string studentid);
        List<ExamDetails> GetExamDetailsBySectionId(int examTypeId, int schoolId, int sectionId);
        List<SubjectList> GetSubjectList(int stdid, int schoolid, int sectionid);
        //List<ExamHistory> GetExamTaskDetails(int StudentId);
        Task<IEnumerable<ExamHistory>> GetExamTaskDetails(int StudentId);
        Task<IEnumerable<TestHistory>> GetTestDetails(int studentid, int subjectid);
    }
    public class ExamDetailsServices : IExamDetailsServices
    {
        private List<ExamDetails> _students = new List<ExamDetails>
        {
            new ExamDetails { ExamDetailId = 1, ExamTypeId=1, ExamDate = DateTime.Now,SubjectId=1,
                Marks=1,  EntryBy = "test", EntryDate =DateTime.Now}
        };

        //private List<ExamHistory> result;
        private readonly IConfiguration _configuration;

        private readonly AppSettings _appSettings;

        public ExamDetailsServices(IOptions<AppSettings> appSettings, IConfiguration configuration)
        {
            _appSettings = appSettings.Value;


            _configuration = configuration;

        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _students.SingleOrDefault(x => x.EntryBy == model.Email && x.EntryBy == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<ExamDetails> GetAll()
        {

            List<ExamDetails> studentProfileViews = new List<ExamDetails>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM ExamDetails";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                ExamDetails studentProfileView = new ExamDetails();

                studentProfileView.ExamDetailId = Convert.ToInt32(reader["ExamDetailId"]);
                studentProfileView.ExamTypeId = Convert.ToInt32(reader["ExamTypeId"]);
                studentProfileView.ExamDate = Convert.ToDateTime(reader["ExamDate"]);
                studentProfileView.SubjectId = Convert.ToInt32(reader["SubjectId"]);
                studentProfileView.Marks = Convert.ToInt32(reader["Marks"]);
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.EntryDate = string.IsNullOrEmpty(reader["EntryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["EntryDate"]);


                studentProfileViews.Add(studentProfileView);
            }
            reader.Close();
            connection.Close();
            return studentProfileViews;
            //return _students;
        }

        public ExamDetails GetById(int id)
        {
            ExamDetails studentProfileView = new ExamDetails();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "Select * FROM ExamDetails where ExamDetailId=" + id + "";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                studentProfileView.ExamDetailId = Convert.ToInt32(reader["ExamDetailId"]);
                studentProfileView.ExamTypeId = Convert.ToInt32(reader["ExamTypeId"]);
                studentProfileView.ExamDate = Convert.ToDateTime(reader["ExamDate"]);
                studentProfileView.SubjectId = Convert.ToInt32(reader["SubjectId"]);
                studentProfileView.Marks = Convert.ToInt32(reader["Marks"]);
                studentProfileView.EntryBy = reader["EntryBy"].ToString();
                studentProfileView.EntryDate = string.IsNullOrEmpty(reader["EntryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["EntryDate"]);
            }
            return studentProfileView;

            //return _students.FirstOrDefault(x => x.taskId == id);
        }


        public string GetExamDetails(int id)
        {
            string connectionString = _configuration.GetConnectionString("StudentDB");

            var oStudents = "";

            using (IDbConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();
                oStudents = con.Query<string>(@"SELECT JSON_QUERY(( 
		                SELECT  ED.ExamDetailId AS id, ExamTitle AS title, ExamDate AS exam_date, ExamSyllabus AS exam_syllabus, ISNULL(ER.GetMarks, 0) AS marks, ISNULL(ExamVenue, '') AS venue,
		                ExamStart AS start_time, ExamEnd AS end_time, ISNULL(ES.ExamSetupID,0) AS exam_type, ISNULL(ES.ExamType, '') AS exam_type_name, ISNULL(SC.SchoolId,0) AS school, 
		                ISNULL(SC.SchoolName, '') AS school_name, ISNULL(C.sl, 0) AS assigned_class, ISNULL(C.name, '') AS assigned_class_name, ISNULL(S.SectionId, 0) AS section, 
		                ISNULL(S.SectionName, '') AS section_name, ISNULL(SB.SubjecId, 0) AS subject, ISNULL(SB.SubjectName, '') AS subject_name, ED.EntryBy AS created_by_email, 
		                ISNULL(ED.Updatedby, '') AS updated_by_email, ED.EntryDate AS created_at, ISNULL(ED.UpdatedDate, 0) AS updated_at FROM ExamDetails ED 
		                LEFT JOIN ExamResult ER ON ED.ExamDetailId = ER.ExamDetailId AND ED.SchoolId = ER.SchoolId AND ED.ClassID = ER.ClassId AND ED.SectionID = ER.SectionId
		                LEFT JOIN ExamSetup ES ON ED.ExamTypeId = ES.ExamSetupID AND ED.SchoolId = ES.SchoolId
		                LEFT JOIN SubscriberSchools SC ON ED.SchoolId = SC.SchoolId
		                LEFT JOIN Class C ON ED.ClassID = C.sl AND ED.SchoolId = C.SchoolId
		                LEFT JOIN Sections S ON ED.SectionID = S.SectionId AND ED.SchoolId = S.SchoolId
		                LEFT JOIN Subjects SB ON ED.SubjectId = SB.SubjecId AND ED.SchoolId = S.SchoolId 
		                WHERE [ER].[StudentId] = '" + id + "' " +
                        " FOR JSON PATH, WITHOUT_ARRAY_WRAPPER )) AS [ExamSchedule]").FirstOrDefault();

                con.Close();
            }
            return oStudents;
        }


        public List<ExamSetup> GetExamType(string id)
        {
            List<ExamSetup> studentExamViews = new List<ExamSetup>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT ExamSetupID, ExamType, StartDate, EndDate, S.StudentID, ES.SchoolId, S.ClassId, S.SectionId FROM ExamSetup ES " +
                "LEFT JOIN Students S on ES.SchoolId = S.SchoolId Where ES.IsActive = 1 AND S.StudentID = '" + id + "'  " +
                "AND EXISTS (SELECT * FROM ExamDetails WHERE ExamTypeId = ES.ExamSetupID AND SchoolId = ES.SchoolId AND SectionID = S.SectionId) ";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                ExamSetup studentExamView = new ExamSetup();
                studentExamView.ExamSetupID = Convert.ToInt32(reader["ExamSetupID"]);
                studentExamView.StudentId = Convert.ToInt32(reader["StudentID"]);
                studentExamView.SchoolId = Convert.ToInt32(reader["SchoolId"]);
                studentExamView.SectionId = Convert.ToInt32(reader["SectionId"]);
                studentExamView.ExamType = reader["ExamType"].ToString();

                studentExamViews.Add(studentExamView);
            }
            reader.Close();
            connection.Close();
            return studentExamViews;
        }


        public List<ExamDetails> GetExamDetailsBySectionId(int examTypeId, int schoolId, int sectionId)
        {
            List<ExamDetails> studentExamDetailsViews = new List<ExamDetails>();

            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT ED.*, C.name, S.SectionName FROM ExamDetails ED LEFT JOIN Class C ON ED.ClassID = C.sl AND ED.SchoolId = C.SchoolId " +
                "LEFT JOIN Sections S ON ED.SectionID = S.SectionId AND ED.ClassID = S.ClassId AND ED.SchoolId = S.SchoolId " +
                "WHERE ED.ExamTypeId = '" + examTypeId + "' AND ED.SchoolId = '" + schoolId + "' AND ED.SectionId = '" + sectionId + "' ORDER BY ExamDate ASC";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                ExamDetails studentExamDetailsView = new ExamDetails();
                studentExamDetailsView.ExamDetailId = Convert.ToInt32(reader["ExamDetailId"]);
                studentExamDetailsView.SchoolId = Convert.ToInt32(reader["SchoolId"]);
                studentExamDetailsView.ExamTypeId = Convert.ToInt32(reader["ExamTypeId"]);
                studentExamDetailsView.ExamDate = Convert.ToDateTime(reader["ExamDate"]);
                studentExamDetailsView.ExamTitle = reader["ExamTitle"].ToString();
                studentExamDetailsView.SubjectId = Convert.ToInt32(reader["SubjectId"]);
                studentExamDetailsView.SectionID = Convert.ToInt32(reader["SectionId"]);
                studentExamDetailsView.ClassID = Convert.ToInt32(reader["ClassID"]);
                studentExamDetailsView.Marks = Convert.ToInt32(reader["Marks"]);
                studentExamDetailsView.ExamVenue = reader["ExamVenue"].ToString();

                DateTime start_time = DateTime.ParseExact(reader["ExamStart"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                studentExamDetailsView.ExamStart = start_time.ToString("hh:mm tt");

                DateTime end_time = DateTime.ParseExact(reader["ExamEnd"].ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                studentExamDetailsView.ExamEnd = end_time.ToString("hh:mm tt");

                studentExamDetailsView.EntryBy = reader["EntryBy"].ToString();
                studentExamDetailsView.EntryDate = string.IsNullOrEmpty(reader["EntryDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["EntryDate"]);
                studentExamDetailsView.Updatedby = reader["Updatedby"].ToString();

                studentExamDetailsView.UpdatedDate = string.IsNullOrEmpty(reader["UpdatedDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedDate"]);

                studentExamDetailsView.IsActive = Convert.ToInt32(reader["IsActive"].ToString());

                studentExamDetailsViews.Add(studentExamDetailsView);

            }
            reader.Close();
            connection.Close();
            return studentExamDetailsViews;
        }


        public List<SubjectList> GetSubjectList(int stdid, int schoolid, int sectionid)
        {
            List<SubjectList> subjectslist = new List<SubjectList>();
            string connectionString = _configuration.GetConnectionString("StudentDB");
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT s.SubjecId, s.SubjectName, s.SchoolId, s.ClassId, std.SectionId FROM Subjects s " +
                "LEFT JOIN Classes c ON c.ClassId = s.ClassId LEFT JOIN Students std ON std.ClassId = c.ClassId " +
                "WHERE std.StudentID = '" + stdid + "' AND s.SchoolId = '" + schoolid + "' AND std.SectionId = '" + sectionid + "' " +
                "AND EXISTS(SELECT* FROM ExamDetails ED WHERE ED.SubjectId = s.SubjecId AND ED.ClassID = s.ClassId AND ED.SchoolId = s.SchoolId)  ";
            SqlCommand com = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                SubjectList subject = new SubjectList
                {
                    SubjecId = Convert.ToInt32(reader["SubjecId"]),
                    SubjectName = reader["SubjectName"].ToString(),
                    SchoolId = Convert.ToInt32(reader["SchoolId"]),
                    ClassId = Convert.ToInt32(reader["ClassId"]),
                    Sectionid = Convert.ToInt32(reader["SectionId"])
                };

                subjectslist.Add(subject);
            }

            return subjectslist;
        }


        //public async Task<IEnumerable<ExamHistory>> GetExamTaskDetails(int studentid)
        //{
        //    const string query = @"SELECT ExamSetupID, ExamType, SJ.SubjecId, SJ.SubjectName, vE.ExamTitle, vE.ExamDate, vE.ExamTypeId, vE.ExamSyllabus, " +
        //        "vE.ExamVenue, vE.ExamStart, vE.ExamEnd, vE.Marks, vE.FullMarks, vE.GetMarks, vE.ClassName, vE.SectionName FROM ExamSetup ES " +
        //        "LEFT JOIN ExamDetails ED ON ES.ExamSetupID = ED.ExamTypeId AND ES.SchoolId = ED.SchoolId " +
        //        "LEFT JOIN Students S on ES.SchoolId = S.SchoolId AND S.SectionID = ED.SectionId " +
        //        "LEFT JOIN Subjects SJ ON ES.SchoolId = SJ.SchoolId AND ED.SubjectId = SJ.SubjecId " +
        //        "LEFT JOIN Classes C ON SJ.SchoolId = C.SchoolId AND C.ClassId = SJ.ClassId AND ED.ClassID = SJ.ClassId " +
        //        "LEFT JOIN vwExamResultDetails vE ON SJ.SubjecId = vE.SubjectId WHERE ES.IsActive = 1 AND S.StudentID = @StudentID";

        //    string connectionString = _configuration.GetConnectionString("StudentDB");
        //    using (var conn = new SqlConnection(connectionString))
        //    {
        //        var directorDictionary = new Dictionary<int, ExamHistory>();

        //        var result = await conn.QueryAsync<ExamHistory, Subjects, ExamDetails, ExamHistory>(
        //            query, (m, n, p) =>
        //             {
        //                 ExamHistory examHistory;

        //                 if (!directorDictionary.TryGetValue(m.ExamSetupID, out examHistory))
        //                 {
        //                     directorDictionary.Add(examHistory.ExamSetupID, examHistory = m);
        //                 }

        //                 //country
        //                 if (examHistory.Subjects == null)
        //                 {
        //                     if (n == null)
        //                     {
        //                         n = new Subjects { SubjectName = "" };
        //                     }
        //                     examHistory.Subjects = (IList<SubjectLst>)n;
        //                 }


        //                 //books
        //                 if (examHistory.Subjects.Exa == null)
        //                 {
        //                     personEntity.Books = new List<Book>();
        //                 }

        //                 if (book != null)
        //                 {
        //                     if (!personEntity.Books.Any(x => x.BookId == book.BookId))
        //                     {
        //                         personEntity.Books.Add(book);
        //                     }
        //                 }

        //                 if (!directorDictionary.TryGetValue(n.SubjecId, out SubjectLst subject))
        //                 {
        //                     subject = n;
        //                     subject.ExamDetails = new List<ExamDetails>();

        //                     directorDictionary.Add(subject.SubjecId, subject);
        //                 }

        //                 subject.ExamDetails.Add(p);
        //                 return examHistory;
        //             },
        //            splitOn: "SubjecId, ExamTitle", param: new { @StudentID = studentid });

        //        return result.Distinct();
        //    }
        //}


        public async Task<IEnumerable<ExamHistory>> GetExamTaskDetails(int studentid)
        {
            const string query = @"SELECT ExamSetupID, ExamType, SJ.SubjecId, SJ.SubjectName, vE.ExamTitle, vE.ExamDate, vE.ExamTypeId, vE.ExamSyllabus, " +
                "vE.ExamVenue, vE.ExamStart, vE.ExamEnd, vE.Marks, vE.FullMarks, vE.GetMarks, vE.ClassName, vE.SectionName FROM ExamSetup ES " +
                "LEFT JOIN ExamDetails ED ON ES.ExamSetupID = ED.ExamTypeId AND ES.SchoolId = ED.SchoolId " +
                "LEFT JOIN Students S on ES.SchoolId = S.SchoolId AND S.SectionID = ED.SectionId " +
                "LEFT JOIN Subjects SJ ON ES.SchoolId = SJ.SchoolId AND ED.SubjectId = SJ.SubjecId " +
                "LEFT JOIN Classes C ON SJ.SchoolId = C.SchoolId AND C.ClassId = SJ.ClassId AND ED.ClassID = SJ.ClassId " +
                "LEFT JOIN vwExamResultDetails vE ON SJ.SubjecId = vE.SubjectId AND S.StudentID=vE.StudentID WHERE ES.IsActive = 1 AND S.StudentID = @StudentID ";
                
                //"AND EXISTS (SELECT * FROM vwExamResultDetails vE WHERE S.SchoolId = vE.SchoolId AND S.ClassId = vE.ClassId AND S.SectionID = vE.SectionId " +
                //"AND SJ.SubjecId = vE.SubjectId AND S.StudentID = vE.StudentID)";

            string connectionString = _configuration.GetConnectionString("StudentDB");
            using (var conn = new SqlConnection(connectionString))
            {
                var lookup = new Dictionary<int, ExamHistory>();

                var result = await conn.QueryAsync<ExamHistory, SubjectLst, ExamDetails, ExamHistory>(
                    query, (m, n, p) =>
                    {
                        ExamHistory examReport;
                        if (!lookup.TryGetValue(m.ExamSetupID, out examReport)) lookup.Add(m.ExamSetupID, examReport = m);

                        examReport.Subjects ??= new List<SubjectLst>();
                        if (!examReport.Subjects.Any(c => c.SubjectName == n.SubjectName)) examReport.Subjects.Add(n);

                        SubjectLst subjects = examReport.Subjects.Where(x => x.SubjectName == n.SubjectName).FirstOrDefault();
                        subjects.ExamDetails ??= new List<ExamDetails>();

                        if (!subjects.ExamDetails.Contains(p))
                            subjects.ExamDetails.Add(p);

                        return examReport;
                    },
                    splitOn: "SubjecId, ExamTitle",
                    param: new { @StudentID = studentid });

                return result.Distinct();
            }
        }

        public async Task<IEnumerable<TestHistory>> GetTestDetails(int studentid, int subjectid)
        {
            const string query = @"SELECT T.TaskId, TaskTypeName, SubjecId, SubjectName, TeacherName, TaskDetails, T.SectionId, 
                            TaskNumber, FullMarks, GPA, TestExamResultPublished, ExamSession, ResultDate, EntryDate, GetMarks, StudentId, Remarks FROM vw_Tasks T 
                            Inner JOIN TestExamResult TR ON T.TaskId = TR.TaskId WHERE StudentId=@StudentId AND SubjecId=@SubjecId";

            string connectionString = _configuration.GetConnectionString("StudentDB");
            using (var conn = new SqlConnection(connectionString))
            {
                var lookupDictionary = new Dictionary<int, TestHistory>();

                var result = await conn.QueryAsync<TestHistory, TestExamResult, TestHistory>(
                    query, (th, ter) =>
                    {
                        if (!lookupDictionary.TryGetValue(th.TaskId, out TestHistory testReport))
                        {
                            testReport = th;
                            testReport.TestExamResults = new List<TestExamResult>();
                            lookupDictionary.Add(testReport.TaskId, testReport);
                        }
                        testReport.TestExamResults.Add(ter);
                        return testReport;
                    },
                    splitOn: "TaskNumber",
                    param: new { @StudentID = studentid, @SubjecId = subjectid });

                return result.Distinct();
            }
        }






        // helper methods
        private string generateJwtToken(ExamDetails user)
        {
            // generate token that is valid for 3 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("ExamDetailId", user.ExamDetailId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }








        #region For Dapper SplitOn
        //public static List<HistoryReport> GetHistoryReport(int areaId, int year)
        //{
        //    string qry = "Select A.type,SA.id SubAreaId,SA.name SubArea,E.name,E.tag,AC.name ActivityName,AC.id,AC.frequency," +
        //        "AC.frequencyType,H.shift,H.startTime,H.endTime,H.partsChange,H.currentPhase,HE.vLastName CreatedByName,HE.vFileForSignature CreatedBySignature " +
        //        "From MaintenanceHistory H left join MaintenanceSchedule S On S.id = H.scheduleId " +
        //        "left join MaintenanceActivity AC On AC.id = S.activityId " +
        //        "left join MaintenanceEquipment E On E.id = AC.equipmentId " +
        //        "left join MaintenanceSubArea SA On SA.id = E.subAreaId " +
        //        "left join MaintenanceArea A On A.id = SA.areaId " +
        //        "left join User2 U On U.vUserId = H.createdBy " +
        //        "left join HRMEmployee HE On HE.cEmployeeId = U.cEmployeeId " +
        //        "Where H.status = 3 And Year(S.date) = @year And areaId = @areaId And H.checkedBy is not null";

        //    string connectionString = _configuration.GetConnectionString("StudentDB");
        //    using IDbConnection con = new SqlConnection(connectionString);

        //    var lookup = new Dictionary<int, HistoryReport>();

        //    con.Query<HistoryReport, Equipment, Activity, History, HistoryReport>(qry, (m, n, p, q) =>
        //    {
        //        HistoryReport historyReport;
        //        if (!lookup.TryGetValue(m.SubAreaId, out historyReport)) lookup.Add(m.SubAreaId, historyReport = m);

        //        historyReport.Equipments ??= new List<Equipment>();
        //        if (!historyReport.Equipments.Any(c => c.Name == n.Name)) historyReport.Equipments.Add(n);
        //        //q.StartTime.ToString("MMMM")
        //        Equipment equipment = historyReport.Equipments.Where(x => x.Name == n.Name).FirstOrDefault();
        //        equipment.Activities ??= new Dictionary<string, IDictionary<string, IList<string>>>();
        //        if (!equipment.Activities.ContainsKey(p.ActivityName + "---" + string.Join(",", p.Frequency, p.FrequencyType))) equipment.Activities.Add(p.ActivityName + "---" + string.Join(",", p.Frequency, p.FrequencyType), new Dictionary<string, IList<string>>());
        //        if (!equipment.Activities[p.ActivityName + "---" + string.Join(",", p.Frequency, p.FrequencyType)].ContainsKey(q.StartTime.ToString("MMMM"))) equipment.Activities[p.ActivityName + "---" + string.Join(",", p.Frequency, p.FrequencyType)].Add(q.StartTime.ToString("MMMM"), new List<string>());
        //        equipment.Activities[p.ActivityName + "---" + string.Join(",", p.Frequency, p.FrequencyType)][q.StartTime.ToString("MMMM")].Add(q.StartTime.ToString());

        //        equipment.Starts ??= new Dictionary<string, DateTime>();
        //        if (!equipment.Starts.ContainsKey(q.StartTime.ToString("MMMM"))) equipment.Starts.Add(q.StartTime.ToString("MMMM"), q.StartTime);
        //        else
        //        {
        //            int result = DateTime.Compare(equipment.Starts[q.StartTime.ToString("MMMM")], q.StartTime);
        //            if (result > 0) equipment.Starts[q.StartTime.ToString("MMMM")] = q.StartTime;
        //        }

        //        equipment.Ends ??= new Dictionary<string, DateTime>();
        //        if (!equipment.Ends.ContainsKey(q.EndTime.ToString("MMMM"))) equipment.Ends.Add(q.EndTime.ToString("MMMM"), q.EndTime);
        //        else
        //        {
        //            int result = DateTime.Compare(equipment.Ends[q.EndTime.ToString("MMMM")], q.EndTime);
        //            if (result < 0) equipment.Ends[q.EndTime.ToString("MMMM")] = q.EndTime;
        //        }

        //        equipment.Parts ??= new Dictionary<string, string>();
        //        if (!equipment.Parts.ContainsKey(q.StartTime.ToString("MMMM"))) equipment.Parts.Add(q.StartTime.ToString("MMMM"), q.PartsChange);

        //        equipment.CurrentPhases ??= new Dictionary<string, string>();
        //        if (!equipment.CurrentPhases.ContainsKey(q.StartTime.ToString("MMMM")) && q.CurrentPhase != "" && q.CurrentPhase != null) equipment.CurrentPhases.Add(q.StartTime.ToString("MMMM"), q.CurrentPhase);

        //        equipment.Shifts ??= new Dictionary<string, int>();
        //        if (!equipment.Shifts.ContainsKey(q.StartTime.ToString("MMMM"))) equipment.Shifts.Add(q.StartTime.ToString("MMMM"), q.Shift);

        //        equipment.CreatedByNames ??= new Dictionary<string, string>();
        //        if (!equipment.CreatedByNames.ContainsKey(q.StartTime.ToString("MMMM"))) equipment.CreatedByNames.Add(q.StartTime.ToString("MMMM"), q.CreatedByName);

        //        equipment.CreatedBySignatures ??= new Dictionary<string, byte[]>();
        //        if (!equipment.CreatedBySignatures.ContainsKey(q.StartTime.ToString("MMMM"))) equipment.CreatedBySignatures.Add(q.StartTime.ToString("MMMM"), q.CreatedBySignature);

        //        return historyReport;
        //    }, splitOn: "Name,ActivityName,Shift", param: new { @areaId = areaId, @year = year }).AsQueryable();
        //    List<HistoryReport> result = lookup.Values.ToList();
        //    return result;
        //}
        #endregion


    }
}
