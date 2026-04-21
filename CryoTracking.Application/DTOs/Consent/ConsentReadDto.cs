using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.DTOs.Consent
{
    public class ConsentReadDto
    {
        public int ConsentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string ConsentType { get; set; }
        public string FilePath { get; set; }
        public DateTime SignedAt { get; set; }
    }
}
