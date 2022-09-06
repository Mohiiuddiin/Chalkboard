using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Models
{
    public class Guardians
    {
        public int GuardiansId { get; set; }
        public int RelationId { get; set; }
        public int StudentId { get; set; }
        public int GuardianId { get; set; }
        public string Relation { get; set; }
        public string EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public int SchoolId { get; set; }


    }
}
