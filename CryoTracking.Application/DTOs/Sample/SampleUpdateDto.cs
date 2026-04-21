using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.DTOs.Sample
{
   public class SampleUpdateDto
    {
        [Required]
        public string Status { get; set; }

        public DateTime? ThawDate { get; set; }
        public string? Notes { get; set; }
        // PatientId ve SampleType degistirilemez
    }
}
