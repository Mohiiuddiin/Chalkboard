using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class vw_Alpha
    {
        public string TaskId { get; set; }
        public string TaskDetails { get; set; }
        public string TaskDate { get; set; }

        public string TaskTypeName { get; set; }
        //public string StudentId { get; set; }
        public string SubjectName { get; set; }
        public string SectionName { get; set; }
        public int TeacherId { get; set; }
        public int ClassId { get; set; }

        //public int SectionId { get; set; }
        public string EntryBy { get; set; }
        public string TeacherName { get; set; }


        //public virtual Students Students { get; set; }
    }
}
