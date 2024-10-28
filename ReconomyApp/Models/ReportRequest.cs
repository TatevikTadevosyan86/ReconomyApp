public enum ReportType
{
    GeneralInfo,
    Attendance
}

public class ReportRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ReportType ReportType { get; set; } // Change to enum
}
