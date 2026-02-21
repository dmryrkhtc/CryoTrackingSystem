using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Domain.Entities
{
    public class Patient
    {

        //PRIMARY KEY
        public int PatientId { get; set; } 
        public string FullName { get; set; }
        public string TCNo { get; set; }  
        
        //YASA GORE BASARI ORANI
        public DateTime DateOfBirth { get; set; } 
        public string Gender { get; set; }
        public string CoupleType { get; set; }

        //SURE UYARILARI ICIN
        public string ContactInfo { get; set; }

        //BOSANMA DURUMU
        public string MaritalStatus { get; set; }

        //KALITE STANDARTLARI ICIN OLUSTURULAN KAYITLARIN TARIHI
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
