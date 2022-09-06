using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class Exams
    {
		public int ExamId { get; set; }
		public int ClassId { get; set; }
		public string ExamName { get; set; }
		public DateTime StartingDate { get; set; }
		public DateTime ResultDate { get; set; }
		public string EntryBy { get; set; }
		public DateTime EntryDate { get; set; }
	}

}
