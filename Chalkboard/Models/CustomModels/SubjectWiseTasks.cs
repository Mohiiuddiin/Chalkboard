using ChalkboardAPI.Models;
using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChalkboardAPI.Models.CustomModels
{
    public class SubjectWiseTasks
    {
        public string SubjectName { get; set; }
        public List<Tasks> TasktLst { get; set; }
    }
    public class ClassTestResult
    {
        public string SubjectName { get; set; }
        public string AverageMark { get; set; }
    }

    public class SubjectList
    {
        public int SubjecId { get; set; }
        public string SubjectName { get; set; }
        public int SchoolId { get; set; }
        public int ClassId { get; set; }
        public int Sectionid { get; set; }
    }
}
