using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class LoginHistory
    {
        public int LID { get; set; }
        public string MemberID { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string WorkingTimeHr { get; set; }
        public string LoginIP { get; set; }
        public int SchoolId { get; set; }
        public int IsApp { get; set; }
        public string DeviceId { get; set; }
        public string LogType { get; set; }
        public int IsStudentOrParent { get; set; }
    }
}
