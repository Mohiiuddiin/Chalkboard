using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class ExamSetup
    {
		public int ExamSetupID { get; set; }
		public int SchoolId { get; set; }
		public int SectionId { get; set; }
		public string ExamType { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
        public string Remark { get; set; }
        public string EntryBy { get; set; }
		public DateTime EntryDate { get; set; }
		public string Updateby { get; set; }
		public DateTime UpdateDate { get; set; }
		public int IsActive { get; set; }

		public int StudentId { get; set; }
	}
}
