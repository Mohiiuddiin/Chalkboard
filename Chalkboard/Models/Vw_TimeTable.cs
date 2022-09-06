using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class Vw_TimeTable
    {
        public string Day { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string SubjectName { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int ClassesId { get; set; }
        public int SectionId { get; set; }
    }
}
