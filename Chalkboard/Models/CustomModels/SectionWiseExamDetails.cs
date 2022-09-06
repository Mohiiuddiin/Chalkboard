using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESCHOOL.Models;

namespace ChalkboardAPI.Models.CustomModels
{
    public class SectionWiseExamDetails
    {
        public string ExamType { get; set; }
        public List<SubjectList> SubjectList { get; set; }
        public List<ExamDetails> ExamDetailsLst { get; set; }
    }
}
