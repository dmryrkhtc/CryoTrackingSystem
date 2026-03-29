using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Domain.Entities
{
   public class SampleStatus
    {
        public int StatusId { get; set; }
        public int SampleId { get; set; }
        //frozen, thawed, discarded, legal hold gibi durumlar
        public string Status { get; set; }
        // UserId
        public int ActionBy { get; set; }
        //zaman
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        //klinik gozlem notları
        public string? Reason { get; set; }
        // Navigation
        public Sample Sample { get; set; }
    }
}
