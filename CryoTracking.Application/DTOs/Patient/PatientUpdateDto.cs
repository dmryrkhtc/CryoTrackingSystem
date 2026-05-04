using CryoTracking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.DTOs.Patient
{
    public class PatientUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public string ContactInfo { get; set; }
        public string? PartnerFullName { get; set; }
        public string? PartnerTCNo { get; set; }
        public CoupleType CoupleType { get; set; }
        public MaritalStatusType MaritalStatus { get; set; }
        // TCNo ve DateOfBirth kasitli olarak yok — degistirilemez
    }
}
