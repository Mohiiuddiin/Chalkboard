using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class VW_StudentLogin
    {
        public int StudentId { get; set; }
        public string Email { get; set; }
        //public string EmailOrPhone { get; set; }
        public string Password { get; set; }
        public string GuardianEmail { get; set; }
        //public string GuardianEmailOrPhone { get; set; }
        public string GuardianPassword { get; set; }
        public string DeviceId { get; set; }
        public string ClassId  { get; set; }
        public string GuardianDeviceId { get; set; }
        public string SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int LoginActive { get; set; }
        public string BearerToken { get; set; }
        public DateTime? BearerTokenExp { get; set; }
        
    }
}
