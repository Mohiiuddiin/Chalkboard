using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class NewsUpdates
    {
        // public int MsgID { get; set; }
        //public string Headline { get; set; }
        //public string FullNews { get; set; }
        //public string Msgfor { get; set; }
        //public DateTime PublishDate { get; set; }
        //public int SchoolId { get; set; }
        //public int ProjectId { get; set; }
        //public string EntryBy { get; set; }



        public int MsgID { get; set; }
        public string Headline { get; set; }
        public string FullNews { get; set; }
        public string Msgfor { get; set; }
        public DateTime PublishDate { get; set; }
        public string NewsForTeacher { get; set; }
        public string NewsForStudent { get; set; }
        public string NewsForClass { get; set; }
        public string NewsForSection { get; set; }
        public string NewsForSubjectCode { get; set; }
        public int SchoolId { get; set; }
        public int ProjectId { get; set; }
        public string EntryBy { get; set; }

    }
}
