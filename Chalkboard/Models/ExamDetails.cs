using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class ExamDetails
    {
		public int ExamDetailId { get; set; }
        public int SchoolId { get; set; }
        public int ExamTypeId { get; set; }
		public DateTime ExamDate { get; set; }
        public string ExamTitle { get; set; }
        public int ClassID { get; set; }
        public int SectionID { get; set; }
        public int SubjectId { get; set; }
        public string ExamSyllabus { get; set; }
        public int Marks { get; set; }
        public int FullMarks { get; set; }
        public string GetMarks { get; set; }
        public string ExamVenue { get; set; }
        public string ExamStart { get; set; }
        public string ExamEnd { get; set; }
        public string EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public string Updatedby { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int IsActive { get; set; }
		
	}
}
