using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Domain.Entities
{
    public class QualityAssessment
    {
        public int QAId { get; set; }
        public int SampleId { get; set; }
        //embriyo ise blastokist, sperm ise motilite, yumurta ise kalite gibi kriterler
        public string MorphologyGrade { get; set; }
        //klinik gozlem notlari
        public string? EmbryologistNote { get; set; }
        //sistem onerisi
        public double? AIScore { get; set; }
        public DateTime ScoredAt { get; set; } = DateTime.UtcNow;
        // Navigation
        public Sample Sample { get; set; }

    }
}
