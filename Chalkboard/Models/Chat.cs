using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public int taskId { get; set; }
        public int ClassId { get; set; }
        public int SendById { get; set; }
        public string SenderName { get; set; }
        public string ChatDetails { get; set; }
        public string ChatTime { get; set; }
        public string IsRead { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryBy { get; set; }
        public int ProjectId { get; set; }
        public int SchoolId { get; set; }
    }
}
