using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class Studentprofile
    {

        public int StudentprofileId { get; set; }
        public string StudentName { get; set; }
        public int StudentID { get; set; }
        public string Standard { get; set; }
        public string GuardianName { get; set; }
        public DateTime DOB { get; set; }
        public string BloodGroup { get; set; }
        public int MobileNo { get; set; }
        public string Division { get; set; }

    }
}
