using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReconomyApp.Data;
using ReconomyApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReconomyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInOutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CheckInOutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/checkinout/checkin
        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInOutRecord record)
        {
            if (record == null || record.ParticipantId <= 0)
            {
                return BadRequest("Invalid check-in data");
            }
            // Check if the participant exists
            var participantExists = await _context.Participants.AnyAsync(p => p.ID == record.ParticipantId);
            if (!participantExists)
            {
                return NotFound($"Participant with ID {record.ParticipantId} not found.");
            }
            record.CheckInTime = DateTime.UtcNow; // Set check-in time to now

            _context.CheckInOutRecords.Add(record);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCheckInOutRecords), new { id = record.ID }, record);
        }

        // POST: api/checkinout/checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] CheckInOutRecord record)
        {
            if (record == null || record.ParticipantId <= 0)
            {
                return BadRequest("Invalid check-out data");
            }

            var existingRecord = await _context.CheckInOutRecords
                .Where(r => r.ParticipantId == record.ParticipantId)
                .OrderByDescending(r => r.CheckInTime)
                .FirstOrDefaultAsync();

            if (existingRecord == null || existingRecord.CheckOutTime.HasValue)
            {
                return BadRequest("No active check-in record found for this participant.");
            }

            existingRecord.CheckOutTime = DateTime.UtcNow; // Set check-out time to now
            existingRecord.HoursWorked = existingRecord.CheckOutTime.Value - existingRecord.CheckInTime; // Calculate hours worked

            _context.CheckInOutRecords.Update(existingRecord);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/checkinout/records/{participantId}
        [HttpGet("records/{participantId}")]
        public async Task<IActionResult> GetCheckInOutRecords(int participantId)
        {
            var records = await _context.CheckInOutRecords
                .Where(r => r.ParticipantId == participantId)
                .OrderBy(r => r.CheckInTime)
                .ToListAsync();

            return Ok(records);
        }
    }
}
