using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESCHOOL.Models;

namespace Chalkboard.Models.CustomModels
{
    public class Data
    {
        public List<ExamResultsReturnJsonMH> JsonData { get; set; }
    }
    public class ExamResultsReturnJsonMH
    {
        public int? ExamSetupID { get; set; }
        public string ExamType { get; set; } 
        //public int StudentId { get; set; } 
        public Subjects Subjects { get; set; }  

        
    }
    public class Subjects
    {
        
        public List<Results> Results { get; set; }
    }
    public class Results
    {
        public int? SubjectId { get; set; }
        public string SubjectName { get; set; }
        public ExamDetails ExamDetails { get; set; }
        public List<TestDetails> TestDetails { get; set; }
    }

    public class ExamDetails
    {
        public int? ExamDetailId { get; set; }
        public int? SchoolId { get; set; }
        public int? ExamTypeId { get; set; }
        public DateTime? ExamDate { get; set; }
        public string ExamTitle { get; set; }
        public int? ClassID { get; set; }
        public int? SectionID { get; set; }
        public int? SubjectId { get; set; }
        public string ExamSyllabus { get; set; }
        public int? Marks { get; set; }
        public int? FullMarks { get; set; }
        public string GetMarks { get; set; }
        public string ExamVenue { get; set; }
        public string ExamStart { get; set; }
        public string ExamEnd { get; set; }
        public string EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public string Updatedby { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? IsActive { get; set; }

        public string Remarks { get; set; }


    }

    public class TestExamResults
    {
        public int? TestExamResultId { get; set; }        
        public string TaskNumber { get; set; }        
        public int? FullMarks { get; set; }
        public double? GPA { get; set; }
        public int? TestExamResultPublished { get; set; }
        public DateTime? ExamSession { get; set; }
        public DateTime? ResultDate { get; set; }        
        public int? StudentId { get; set; }
        public string GetMarks { get; set; }
        public string Remarks { get; set; } 
    }

    public class TestDetails
    {
        public int? TaskId { get; set; }
        public int? SectionId { get; set; }
        public int? SubjectId { get; set; }
        public string SubjectName { get; set; }        
        public string TaskDetails { get; set; }
        public string TeacherName { get; set; }        
        public string TaskTypeName { get; set; }


        public TestExamResults TestExamResults { get; set; }

    }
}
