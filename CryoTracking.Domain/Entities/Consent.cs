using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Domain.Entities
{
   public class Consent
    {
        public int ConsentId { get; set; }
        //hangi hastaya ait oldugu
        public int PatientId { get; set; }
        //ne turu onay oldugu (embriyo dondurma, sperm dondurma, yumurta dondurma, cozme, transfer gibi)
        public string ConsentType { get; set; }
        //dosya yolu veya onay belgesinin saklandigi yer
        public string FilePath { get; set; }
        //kanit
        public DateTime SignedAt { get; set; }
        // Navigation
        public Patient Patient { get; set; }
    }
}
