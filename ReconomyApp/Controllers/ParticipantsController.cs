using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ReconomyApp.Data;
using ReconomyApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReconomyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParticipantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/participants
        [HttpGet]
        public async Task<IActionResult> GetParticipants()
        {
            var participants = await _context.Participants
                .Include(p => p.CheckInOutRecords) // Include records
                .AsNoTracking() // Improve performance for read-only operations
                .ToListAsync();

            if (!participants.Any())
            {
                return NotFound("No participants found.");
            }

            return Ok(participants);
        }


        // POST: api/participants
        [HttpPost]
        public async Task<IActionResult> AddParticipant([FromBody] Participant participant)
        {
            // Validate participant data
            if (participant == null || string.IsNullOrWhiteSpace(participant.Name))
            {
                return BadRequest("Invalid participant data");
            }

            await _context.Participants.AddAsync(participant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParticipants), new { id = participant.ID }, participant);
        }

        // PUT: api/participants/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParticipant(int id, [FromBody] Participant participant)
        {
            // Validate participant data
            if (participant == null || id != participant.ID || !ParticipantExists(id))
            {
                return BadRequest("Invalid participant data");
            }

            // Update the participant details
            var existingParticipant = await _context.Participants.FindAsync(id);
            if (existingParticipant == null)
            {
                return NotFound($"Participant with ID {id} not found.");
            }

            existingParticipant.Name = participant.Name;
            existingParticipant.StartDate = participant.StartDate;
            existingParticipant.EndDate = participant.EndDate;
            existingParticipant.WorkCommitment = participant.WorkCommitment;

            // Optionally update CheckInOutRecords if provided
            if (participant.CheckInOutRecords != null)
            {
                existingParticipant.CheckInOutRecords = participant.CheckInOutRecords;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound($"Participant with ID {id} not found.");
            }

            return NoContent();
        }
        // DELETE: api/participants/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            var participant = await _context.Participants.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/participants/report
        [HttpGet("report")]
        public async Task<IActionResult> GenerateReport()
        {
            var participants = await _context.Participants
                .Include(p => p.CheckInOutRecords)
                .AsNoTracking() // Improve performance for read-only operations
                .ToListAsync();

            if (!participants.Any())
            {
                return NotFound("No participants found for report.");
            }

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Report");

            // Set headers
            worksheet.Cells[1, 1].Value = "Participant Name";
            worksheet.Cells[1, 2].Value = "Check-In Time";
            worksheet.Cells[1, 3].Value = "Check-Out Time";
            worksheet.Cells[1, 4].Value = "Hours Worked";
            worksheet.Cells[1, 5].Value = "Reason for Early Departure";

            int row = 2; // Start at row 2 for headers
            foreach (var participant in participants)
            {
                if (participant.CheckInOutRecords != null)
                {
                    foreach (var record in participant.CheckInOutRecords)
                    {
                        worksheet.Cells[row, 1].Value = participant.Name; // Participant Name
                        worksheet.Cells[row, 2].Value = record.CheckInTime; // Check-in Time
                        worksheet.Cells[row, 3].Value = record.CheckOutTime; // Check-out Time
                        worksheet.Cells[row, 4].Value = record.HoursWorked; // Hours Worked
                        worksheet.Cells[row, 5].Value = record.ReasonForEarlyDeparture; // Reason for Early Departure
                        row++;
                    }
                }
            }

            var fileStream = new MemoryStream();
            package.SaveAs(fileStream);
            fileStream.Position = 0;

            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
        }

        private bool ParticipantExists(int id)
        {
            return _context.Participants.Any(e => e.ID == id);
        }
    }
}
