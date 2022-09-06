using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChalkboardAPI.Models.CustomModels;
using ESCHOOL.Models;

namespace ChalkboardAPI.Models.CustomModels
{
    public class ExamHistory
    {
        public int ExamSetupID { get; set; } 
        public string ExamType { get; set; }
        public IList<SubjectLst> Subjects { get; set; }
    }

    public class SubjectLst
    {
        public int SubjecId { get; set; }
        public string SubjectName { get; set; }
        public IList<ExamDetails> ExamDetails { get; set; }
    }
}
