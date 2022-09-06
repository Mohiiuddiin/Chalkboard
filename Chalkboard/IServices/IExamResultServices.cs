using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chalkboard.Models.CustomModels;
using ESCHOOL.Models;
namespace ESCHOOL.IServices
{
    public interface IExamResultServices
    {
        List<ExamResult> Gets();
        List<ExamResult> Get(int classId);
        Data GetExamResult(int id);

        //Student Save(Student oStudent);
        //List<vw_Bravo> GetAllResults();
        //List<vw_Bravo> GetResultsByExamId(int studentId);
        //List<vw_Bravo> GetResultByStudentId(int subjectId);
    }
}
