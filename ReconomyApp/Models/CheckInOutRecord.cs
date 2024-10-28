namespace ReconomyApp.Models
{
    public class CheckInOutRecord
    {
        public int ID { get; set; } // Unique identifier for each record
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public TimeSpan? HoursWorked { get; set; }
        public string ReasonForEarlyDeparture { get; set; }

        // Foreign key to Participant
        public int ParticipantId { get; set; }
        
    }
}
