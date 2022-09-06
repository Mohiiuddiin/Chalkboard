using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChalkboardAPI.Models.CustomModels
{
    public class TestHistory
    {
        public int TaskId { get; set; }
        public string TaskTypeName { get; set; }
        public int SubjecId { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public string TaskDetails { get; set; }
        public int SectionId { get; set; }
        public IList<TestExamResult> TestExamResults { get; set; }
    }

    public class TestExamResult
    {
        public string TaskNumber { get; set; }
        public int FullMarks { get; set; }
        public decimal GPA { get; set; }
        public int TestExamResultPublished { get; set; }
        public DateTime? ExamSession { get; set; }
        public DateTime? ResultDate { get; set; }
        public string GetMarks { get; set; }
        public string StudentId { get; set; }
        public string Remarks { get; set; }
    }
}
