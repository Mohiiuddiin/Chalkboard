using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChalkboardAPI.Models
{
    public class vw_Tasks

    {

        public int TaskId { get; set; }
        public string TeacherName { get; set; }
        public int SubjecId { get; set; }
        public string SubjectName { get; set; }
        public string TaskTypeName { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime TaskDate { get; set; }
        public string TaskDetails { get; set; }
        public int SectionId { get; set; }      

    }
}
