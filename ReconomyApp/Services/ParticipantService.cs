using ReconomyApp.Data;
using ReconomyApp.Models;
using System.Collections.Generic;

public class ParticipantService : IParticipantService
{
    private readonly ApplicationDbContext _context;

    public ParticipantService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Participant> GetParticipants()
    {
        // Fetch all participants from the database without any filters
        return _context.Participants.ToList();
    }
}
