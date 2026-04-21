using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.DTOs.QualityAssessment
{
    public class QualityAssessmentCreateDto
    {

        [Required]
        public int SampleId { get; set; }

        [Required]
        public string MorphologyGrade { get; set; }

        public string? EmbryologistNote { get; set; }
        public double? AIScore { get; set; }
    }
}
