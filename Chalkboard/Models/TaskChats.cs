using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class TaskChats
    {
		public int ChatId { get; set; }
		public int ParentChatId { get; set; }
		public int TaskId { get; set; }
		public int TeacherId { get; set; }
		public int StudentId { get; set; }
		public string CommentDetail { get; set; }
		public DateTime EntryDate { get; set; }
		public string EntryBy { get; set; }
	}
}
