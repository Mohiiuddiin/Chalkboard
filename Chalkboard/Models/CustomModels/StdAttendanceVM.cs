using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chalkboard.Models.CustomModels
{
    public class StdAttendanceVM
    {
        public int StdId { get; set; }
        public string StdName { get; set; }
        public string SchoolName { get; set; }
        public string SectionName { get; set; }
        public string ClassName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StdAttDate { get; set; }
        public string Status { get; set; }
        public string Month { get; set; }
        //public string MyProperty { get; set; }
    }
}
