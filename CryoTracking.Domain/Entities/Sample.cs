using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Domain.Entities
{
    public class Sample
    {
        public int SampleId { get; set; }
        //Kime ait
        public int PatientId { get; set; }
        // Embriyo / sperm / yumurta mi
        public string SampleType { get; set; } 
        //dondurma baslar baslamaz 5 yildan duser
        public DateTime FreezeDate { get; set; }
        //cozdurma kaydi
        public DateTime? ThawDate { get; set; } 
        // Frozen, Thawed, Discarded, LegalHold
        public string Status { get; set; }
        // Embriyolog UserId
        public int CreatedBy { get; set; } 
        //klinik gozlem notları
        public string? Notes { get; set; }
        // Navigation
        public Patient Patient { get; set; }
        public StorageLocation StorageLocation { get; set; }
        public ICollection<SampleStatus> StatusHistory { get; set; }
        public ICollection<QualityAssessment> QualityAssessments { get; set; }
    }
}
