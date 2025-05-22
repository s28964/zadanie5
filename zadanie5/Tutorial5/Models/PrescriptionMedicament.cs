using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tutorial5.Models;

[Table("PrescriptionMedicament")]
[PrimaryKey(nameof(IdPrescription), nameof(IdMedicament))]
public class PrescriptionMedicament
{
    public int IdPrescription { get; set; }
    public Prescription Prescription { get; set; }
    
    public int IdMedicament { get; set; }
    public Medicament Medicament { get; set; }
    
    public int Dose { get; set; }
    
    [MaxLength(100)]
    public string Description { get; set; }
}