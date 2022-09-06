using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChalkboardAPI.Models
{
    public class vW_Echo
    {
        public string TaskId { get; set; }
        public int SubjectId { get; set; }
		public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public string TaskDetails { get; set; }
        public string StudentNameE { get; set; }
        public string CommentDetail { get; set; }
        

    }
}
