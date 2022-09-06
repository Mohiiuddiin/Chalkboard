using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class Vw_TeacherNew
    {
        public int SubjectId { get; set; }

        public int SectionId { get; set; }
        public string SubjectName { get; set; }
        public string BookSt { get; set; }
        public string TeacherName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int TeacherId { get; set; }
        public int ClassesId { get; set; }

    }
}
