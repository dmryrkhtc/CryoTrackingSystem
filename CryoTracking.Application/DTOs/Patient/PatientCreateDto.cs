using CryoTracking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.DTOs.Patient
{
   public class PatientCreateDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }=default!;

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string TCNo { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public CoupleType CoupleType { get; set; }

        [Required]
        public string? ContactInfo { get; set; }
        public string? PartnerFullName { get; set; }
        public string? PartnerTCNo { get; set; }
        public MaritalStatusType MaritalStatus { get; set; }
    }
}
