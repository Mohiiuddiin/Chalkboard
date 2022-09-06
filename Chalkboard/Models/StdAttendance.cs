using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ESCHOOL.Models
{
    public class StdAttendance
    {
        public int StdAttendanceId { get; set; }
        public string StdId { get; set; }
        public string StdAttClassId { get; set; }
        public string StdAttSectionId { get; set; }
        public DateTime StdAttDate { get; set; }
        public string StdStatus { get; set; }
        public string StdAttEntryBy { get; set; }
        public string MonthName { get; set; }
        public int SchoolId { get; set; }


        public string January { get; set; }
        public string February { get; set; }
        public string March { get; set; }
        public string April { get; set; }
        public string May { get; set; }
        public string June { get; set; }
        public string July { get; set; }
        public string August { get; set; }
        public string September { get; set; }
        public string October { get; set; }
        public string November { get; set; }
        public string December { get; set; }
    }
}
