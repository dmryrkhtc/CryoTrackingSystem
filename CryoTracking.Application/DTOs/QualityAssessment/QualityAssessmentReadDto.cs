using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.DTOs.QualityAssessment
{
    public class QualityAssessmentReadDto
    {
        public int QAId { get; set; }
        public int SampleId { get; set; }
        public string MorphologyGrade { get; set; }
        public string? EmbryologistNote { get; set; }
        public double? AIScore { get; set; }
        public DateTime ScoredAt { get; set; }
    }
}
