using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class ExamResult
    {
        public int ExamResultId { get; set; }
        public int ExamDetailId { get; set; }
        public int SchoolId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int FullMarks { get; set; }
        public double GPA { get; set; }
        public int ResultPublished { get; set; }
        public DateTime ExamSession { get; set; }
        public DateTime ResultDate { get; set; }
        public DateTime EntryDate { get; set; }
        public int StudentId { get; set; }
        public string GetMarks { get; set; }
        public string Remarks { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ExamDetails ExamDetails { get; set; }
    }
}
