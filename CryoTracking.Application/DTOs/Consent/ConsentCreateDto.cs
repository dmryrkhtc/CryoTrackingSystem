using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.DTOs.Consent
{
    public class ConsentCreateDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public string ConsentType { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public DateTime SignedAt { get; set; }
    }
}
