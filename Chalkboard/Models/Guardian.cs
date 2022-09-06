using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
	public class Guardian
	{
		public int GuardianId { get; set; }
		public int StudentId { get; set; }
		public string GuardianName { get; set; }
		public string Relationship { get; set; }
		public string MobNo { get; set; }
		public string Email { get; set; }
		public int Password { get; set; }
		public int PhotoId { get; set; }
		public string EntryBy { get; set; }
		public DateTime EntryDate
		{
			get; set;
		}
	}
}
