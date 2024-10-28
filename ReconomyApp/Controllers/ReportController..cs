using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using System.Linq;
using ReconomyApp.Models;
using Microsoft.EntityFrameworkCore;
using ReconomyApp.Data;

namespace ReconomyApp.Controllers
{
    [Route("api/reports")] // Add the route prefix for the controller
    [ApiController]
    public class ReportController : ControllerBase // Use ControllerBase for API controllers
    {
        private readonly IParticipantService _participantService;
        private readonly ApplicationDbContext _context; // Correctly defined here

        // Constructor updated to include ApplicationDbContext
        public ReportController(IParticipantService participantService, ApplicationDbContext context)
        {
            _participantService = participantService;
            _context = context; // Correctly initialized here
        }

        [HttpPost("generate")]
        public IActionResult GenerateReport([FromBody] ReportRequest reportRequest)
        {
            try
            {
                switch (reportRequest.ReportType)
                {
                    case ReportType.GeneralInfo:
                        return GenerateGeneralInfoReport(reportRequest);
                    case ReportType.Attendance:
                        return GenerateAttendanceReport(reportRequest);
                    default:
                        return BadRequest("Invalid report type specified.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // Existing method for General Information Report
        [HttpPost("general")] // Specify the route for this method
        public IActionResult GenerateGeneralInfoReport([FromBody] ReportRequest reportRequest)
        {
            try
            {
                var participants = _participantService.GetParticipants();

                // Create a new Excel package
                using (var package = new ExcelPackage())
                {
                    // Add a worksheet to the Excel package
                    var worksheet = package.Workbook.Worksheets.Add("General Information Report");

                    // Set headers for the columns
                    worksheet.Cells[1, 1].Value = "Name";
                    worksheet.Cells[1, 2].Value = "Start Date";
                    worksheet.Cells[1, 3].Value = "End Date";
                    worksheet.Cells[1, 4].Value = "Work Commitment";

                    // Loop through the participants and add them to the worksheet
                    for (int i = 0; i < participants.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = participants[i].Name;
                        worksheet.Cells[i + 2, 2].Value = participants[i].StartDate.HasValue
                            ? participants[i].StartDate.Value.ToString("yyyy-MM-dd")
                            : (object)null;
                        worksheet.Cells[i + 2, 3].Value = participants[i].EndDate.HasValue
                            ? participants[i].EndDate.Value.ToString("yyyy-MM-dd")
                            : (object)null;
                        worksheet.Cells[i + 2, 4].Value = participants[i].WorkCommitment;
                    }

                    // Save the Excel package to a memory stream
                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);
                        stream.Position = 0; // Reset stream position

                        // Set content type and return the file
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GeneralInfoReport.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost("attendance")] // Specify the route for this method
        public IActionResult GenerateAttendanceReport([FromBody] ReportRequest reportRequest)
        {
            try
            {
                // Fetch all check-in/out records
                var checkInOutRecords = _context.CheckInOutRecords.ToList();

                // Create a new Excel package
                using (var package = new ExcelPackage())
                {
                    // Add a worksheet to the Excel package
                    var worksheet = package.Workbook.Worksheets.Add("Attendance Report");

                    // Set headers for the columns
                    worksheet.Cells[1, 1].Value = "Name";
                    worksheet.Cells[1, 2].Value = "Check In Time";
                    worksheet.Cells[1, 3].Value = "Check Out Time";
                    worksheet.Cells[1, 4].Value = "Hours Worked";
                    worksheet.Cells[1, 5].Value = "Reason for Early Departure";

                    int rowIndex = 2; // Start from the second row for data

                    // Loop through the check-in/out records
                    foreach (var record in checkInOutRecords)
                    {
                        // Fetch participant name based on ParticipantId
                        var participant = _context.Participants.Find(record.ParticipantId);

                        worksheet.Cells[rowIndex, 1].Value = participant?.Name; // Access the name directly from the fetched participant
                        worksheet.Cells[rowIndex, 2].Value = record.CheckInTime.ToString("yyyy-MM-dd HH:mm:ss");
                        worksheet.Cells[rowIndex, 3].Value = record.CheckOutTime.HasValue
                            ? record.CheckOutTime.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            : (object)null;
                        worksheet.Cells[rowIndex, 4].Value = record.HoursWorked.HasValue
                            ? record.HoursWorked.Value.ToString(@"hh\:mm\:ss") // Format TimeSpan
                            : (object)null;
                        worksheet.Cells[rowIndex, 5].Value = record.ReasonForEarlyDeparture;

                        rowIndex++; // Move to the next row for the next record
                    }

                    // Save the Excel package to a memory stream
                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);
                        stream.Position = 0; // Reset stream position

                        // Set content type and return the file
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AttendanceReport.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
