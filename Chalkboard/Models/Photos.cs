using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class Photos
    {
		public int PhotoID { get; set; }
		public string Description { get; set; }
		public string PhotoURL { get; set; }
		public object img { get; set; }
		public string PhotoType { get; set; }
		public string EntryBy { get; set; }
		public DateTime EntryDate { get; set; }
	}
}
