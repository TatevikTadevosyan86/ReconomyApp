namespace ReconomyApp.Models
{
    public class GeneralReport
    {
        public int ID { get; set; }
        public int ParticipantId { get; set; } // Foreign key
        public DateTime ReportDate { get; set; }
        public string WorkCommitment { get; set; }

        public Participant Participant { get; set; } // Navigation property
    }

}
