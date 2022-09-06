using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class ClassRoutineNew
    {

        public int ClassRoutineId { get; set; }
        public int ClassesId { get; set; }
        public int SectionId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryBy { get; set; }

    }
}
