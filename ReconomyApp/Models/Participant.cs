using System;
using System.Collections.Generic;

namespace ReconomyApp.Models
{
    public class Participant
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string WorkCommitment { get; set; } // General Information

        // One-to-many relationship with CheckInOutRecord
        public ICollection<CheckInOutRecord> CheckInOutRecords { get; set; } = new List<CheckInOutRecord>(); // Initialize here
    }
}
