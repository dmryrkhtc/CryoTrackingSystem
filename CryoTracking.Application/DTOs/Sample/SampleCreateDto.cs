using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.DTOs.Sample
{
   public class SampleCreateDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        // embriyo / sperm / yumurta
        public string SampleType { get; set; } 

        [Required]
        public DateTime FreezeDate { get; set; }

        [Required]
       // Frozen, Thawed, Discarded, LegalHold

        public string Status { get; set; } 
        [Required] 
        // Embriyolog UserId
        public int CreatedBy { get; set; }
        public string? Notes { get; set; }
    }
}
