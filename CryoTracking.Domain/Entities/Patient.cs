using CryoTracking.Domain.Enums;
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
        public Gender Gender { get; set; }
        public CoupleType CoupleType { get; set; }//enum

        //SURE UYARILARI ICIN
        public string? ContactInfo { get; set; }

        //EŞ BİLGİLERİ
        public string? PartnerFullName { get; set; }
        public string? PartnerTCNo { get; set; }

        //BOSANMA DURUMU
        public MaritalStatusType MaritalStatus { get; set; }//enum

        //KALITE STANDARTLARI ICIN OLUSTURULAN KAYITLARIN TARIHI
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //navigation properties
        public ICollection<Sample> Samples { get; set; }
        public ICollection<Consent> Consents { get; set; }
        public bool IsActive { get; set; } = true; // Varsayılan olarak aktif

    }
}
