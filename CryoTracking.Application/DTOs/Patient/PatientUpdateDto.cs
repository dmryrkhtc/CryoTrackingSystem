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

        public string? CoupleType { get; set; }
        public string? MaritalStatus { get; set; }
        // TCNo ve DateOfBirth kasitli olarak yok — degistirilemez
    }
}
