using Microsoft.EntityFrameworkCore;
using Tutorial5.Models;

namespace Tutorial5.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<PrescriptionMedicament>().HasKey( pm => new {pm.IdPrescription, pm.IdMedicament});

        modelBuilder.Entity<PrescriptionMedicament>().HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicaments).HasForeignKey(pm => pm.IdPrescription);
        
        modelBuilder.Entity<PrescriptionMedicament>().HasOne(pm => pm.Medicament)
            .WithMany(m => m.PrescriptionMedicaments).HasForeignKey(pm => pm.IdMedicament);
    }
}