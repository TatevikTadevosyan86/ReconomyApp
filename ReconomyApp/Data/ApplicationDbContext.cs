using Microsoft.EntityFrameworkCore;
using ReconomyApp.Models;  

namespace ReconomyApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<CheckInOutRecord> CheckInOutRecords { get; set; }
        public DbSet<GeneralReport> GeneralReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CheckInOutRecord>()
                  .HasOne<Participant>() // No navigation property to avoid cyclic reference
                .WithMany(p => p.CheckInOutRecords)
                .HasForeignKey(c => c.ParticipantId);
        }
    }
}

