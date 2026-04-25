using CryoTracking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.DTOs.Patient
{
    public class PatientReadDto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; }
        public string TCNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? CoupleType { get; set; }
        public string ContactInfo { get; set; }
        public string? MaritalStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
