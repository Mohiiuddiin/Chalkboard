using ChalkboardAPI.Models;
using ChalkboardAPI.Models.CustomModels;
using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChalkboardAPI.IServices
{
   public interface Ivw_TasksServices
    {
        List<vw_Tasks> Gets();
        List<vw_Tasks> Get(int classId);
        List<Tasks> GetTaskListBySubjectId(int id);
        List<Subjects> GetSubjectsByStudentId(string sid);
        List<ClassTestResult> GetSubjectsTestResultByStudentId(string sid);
    }
}
