using ESCHOOL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChalkboardAPI.Models
{
    public class Subjects
    {
        public int SubjecId { get; set; }
        public int ClassId { get; set; }
        public string SubjectName { get; set; }
        public string AverageMark { get; set; }
        public string BookSt { get; set; }
        public DateTime EntryDate { get; set; }
        public int SchoolId { get; set; }
        public int TeacherID { get; set; }
        public string SubjectCode { get; set; }
        public int Section { get; set; }


        //public IList<ExamDetails> ExamDetails { get; set; }
    }
}
