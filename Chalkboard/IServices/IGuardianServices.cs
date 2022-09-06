using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESCHOOL.Models;

namespace ESCHOOL.IServices
{
    public interface IGuardianServices
    {

        //Student Save(Student oStudent);
        List<Guardian> Gets();
        List<Guardian> Get(int classId);

        //List<vw_Bravo> GetAllResults();
        //List<vw_Bravo> GetResultsByExamId(int studentId);
        //List<vw_Bravo> GetResultByStudentId(int subjectId);
    }

}
