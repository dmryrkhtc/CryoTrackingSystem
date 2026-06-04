using CryoTracking.Application.DTOs.QualityAssessment;
using CryoTracking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.DTOs.Sample
{
    public class SampleReadDto
    {
        public int SampleId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public SampleType SampleType { get; set; }
        public DateTime FreezeDate { get; set; }
        public DateTime? ThawDate { get; set; }
        public StatusType Status { get; set; }
        public string? Notes { get; set; }
        public bool IsWarning { get; set; }
        public bool IsExpired { get; set; }
        public List<QualityAssessmentReadDto> QualityAssessments { get; set; }
    }
}
