using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class Tasks
    {
		public int TaskId { get; set; }
		public int SectionId { get; set; }
		public int SubjectId { get; set; }
		public DateTime TaskDate { get; set; }
		//public string TaskHeadline { get; set; }
		public string TaskDetails { get; set; }
		public string TeacherName { get; set; }
		public string SectionName { get; set; }
		public string TaskTypeName { get; set; }
		public string EntryBy { get; set; }
		public DateTime EntryDate { get; set; }
	}
}
