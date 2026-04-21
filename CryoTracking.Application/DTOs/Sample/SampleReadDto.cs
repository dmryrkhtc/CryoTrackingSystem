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
        public string SampleType { get; set; }
        public DateTime FreezeDate { get; set; }
        public DateTime? ThawDate { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
    }
}
