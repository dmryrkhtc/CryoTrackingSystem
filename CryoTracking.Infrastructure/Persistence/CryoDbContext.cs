using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;    
using CryoTracking.Domain.Entities; 

namespace CryoTracking.Infrastructure.Persistence
{
   public class CryoDbContext : DbContext
    {
        public CryoDbContext(DbContextOptions<CryoDbContext> options) : base(options)
        {
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<StorageLocation> StorageLocations { get; set; }
        public DbSet<SampleStatus> SampleStatuses { get; set; }
        public DbSet<QualityAssessment> QualityAssessments { get; set; }
        public DbSet<Consent> Consents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Log> Logs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Primary Keys
            modelBuilder.Entity<QualityAssessment>()
    .HasKey(q => q.QAId);
            // Sample → StorageLocation (1-1)
            modelBuilder.Entity<StorageLocation>()
                .HasOne(s => s.Sample)
                .WithOne(s => s.StorageLocation)
                .HasForeignKey<StorageLocation>(s => s.SampleId);

            // Sample → SampleStatus (1-N)
            modelBuilder.Entity<SampleStatus>()
                .HasOne(ss => ss.Sample)
                .WithMany(s => s.StatusHistory)
                .HasForeignKey(ss => ss.SampleId);

            // Sample → QualityAssessment (1-N)
            modelBuilder.Entity<QualityAssessment>()
                .HasOne(qa => qa.Sample)
                .WithMany(s => s.QualityAssessments)
                .HasForeignKey(qa => qa.SampleId);

            // Patient → Sample (1-N)
            modelBuilder.Entity<Sample>()
                .HasOne(s => s.Patient)
                .WithMany(p => p.Samples)
                .HasForeignKey(s => s.PatientId);

            // Patient → Consent (1-N)
            modelBuilder.Entity<Consent>()
                .HasOne(c => c.Patient)
                .WithMany(p => p.Consents)
                .HasForeignKey(c => c.PatientId);

            // User → Role (N-1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // User → Log (1-N)
            modelBuilder.Entity<Log>()
                .HasOne(l => l.User)
                .WithMany(u => u.Logs)
                .HasForeignKey(l => l.UserId);
            modelBuilder.Entity<SampleStatus>()
         .HasKey(x => x.StatusId);
        }

    }
}
