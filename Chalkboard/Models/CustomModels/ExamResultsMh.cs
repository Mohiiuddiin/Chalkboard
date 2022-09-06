using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chalkboard.Models.CustomModels
{
    public class ExamResultsMh
    {
		public int? ExamSetupID { get; set; }
		public int? SchoolId { get; set; }
		public int? SectionId { get; set; }
		public string ExamType { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Remark { get; set; }
		public string EntryBy { get; set; }
		public DateTime? EntryDate { get; set; }
		public string Updateby { get; set; }
		public DateTime? UpdateDate { get; set; }
		public int? IsActive { get; set; }

		public int? StudentId { get; set; }


        public int? ExamResultId { get; set; }
        public int? ExamDetailId { get; set; }
        
        public int? ClassId { get; set; }
        
        public int? FullMarks { get; set; }
        public double GPA { get; set; }
        public int? ResultPublished { get; set; }
        public DateTime? ExamSession { get; set; }
        public DateTime? ResultDate { get; set; }
        
        
        public string GetMarks { get; set; }
        public string Remarks { get; set; }

        
        public int? ExamTypeId { get; set; }
        public DateTime? Date { get; set; }
        public string ExamTitle { get; set; }
        public int? ClassID { get; set; }
        public int? SectionID { get; set; }
        public int? SubjectId { get; set; }
        public string ExamSyllabus { get; set; }
        public int? Marks { get; set; }
       
        public string ExamVenue { get; set; }
        public string ExamStart { get; set; }
        public string ExamEnd { get; set; }
        
        public string Updatedby { get; set; }
        public DateTime? UpdatedDate { get; set; }


        public int TaskId { get; set; }
        public int SubjecId { get; set; }
        public int StudentID { get; set; }
        public int TeacherId { get; set; }
        public int TestExamResultPublished { get; set; }
        
        public DateTime? TaskDate { get; set; }
        //public string TaskHeadline { get; set; }
        public string TaskDetails { get; set; }
        public string TeacherName { get; set; }
        public string SectionName { get; set; }
        public string TaskTypeName { get; set; }
        public string StudentNameE { get; set; }
        public string SchoolName { get; set; }
        public string SubjectName { get; set; }
        public string Stat { get; set; }

        public int? TestExamResultId { get; set; }
        
        public string TaskNumber { get; set; }
        
        //public decimal? Gpa { get; set; }
        //public int? TestExamResultPublished { get; set; }
        public string stat { get; set; }
        



    }
}
